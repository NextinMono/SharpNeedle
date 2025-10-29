namespace SharpNeedle.Framework.BIXF;

using System.Xml;
using System.Buffers.Binary;
public class BIXF : ResourceBase, IBinarySerializable
{
    public Endianness Endianness { get; set; }
    public uint Signature { get; set; } = BinaryHelper.MakeSignature<uint>("BIXF");
    public int Field04 { get; set; }
    public int Field0C { get; set; }
    public List<string> Table { get; set; } = [];
    public List<Node> Instructions { get; set; } = [];

    public override void Read(IFile file)
    {
        Name = file.Name;
        BaseFile = file;

        using BinaryObjectReader reader = new(file.Open(), StreamOwnership.Transfer, Endianness.Little);
        Read(reader);
    }
    public override void Write(IFile file)
    {
        Name = file.Name;
        BaseFile = file;

        using BinaryObjectWriter writer = new(file.Open(FileAccess.Write), StreamOwnership.Transfer, Endianness);
        Write(writer);
    }
    public void Read(BinaryObjectReader reader)
    {
        var signature = reader.ReadNative<uint>();
        if (signature == BinaryPrimitives.ReverseEndianness(Signature))
        {
            Endianness = Endianness.Big;
        }
        reader.Endianness = Endianness;

        Field04 = reader.ReadInt32();
        int instructionChunkSize = reader.ReadInt32();
        Field0C = reader.ReadInt32();
        int tableEntryCount = reader.ReadInt32();
        reader.Seek(3 + instructionChunkSize, SeekOrigin.Current);

        Table = reader.ReadStringArray(StringBinaryFormat.NullTerminated, tableEntryCount).ToList();

        reader.Seek(20, SeekOrigin.Begin);
        long start = reader.Position;
        while (reader.Position - start < instructionChunkSize)
        {
            Instructions.Add(reader.ReadObject<Node>());
        }
    }
    string NodeIDToString(List<string> IDTable, byte id)
    {
        string value = "Node" + id.ToString();

        if (id < IDTable.Count)
            value = IDTable[id];

        return value;
    }

    public virtual XmlDocument ToXml(List<string> IDTable, List<string> ValueTable)
    {
        var doc = new XmlDocument();
        XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "", "");
        doc.AppendChild(decl);

        string currentParameter = "";
        XmlElement parentElem = null;
        XmlElement currentElem = null;

        foreach (var instr in Instructions)
        {
            switch (instr.ID)
            {
                case Instruction.GoToParent:
                {
                    currentElem = parentElem;
                    if (currentElem != null && currentElem.ParentNode is XmlElement p)
                        parentElem = p;
                    else
                        parentElem = null;
                    break;

                }
                case Instruction.NewParameter:
                {
                    currentParameter = Table[(byte)instr.Value];
                    break;
                }
                case Instruction.NewParameterTable:
                {
                    currentParameter = NodeIDToString(IDTable, instr.Value);
                    break;
                }
                case Instruction.NewValue:
                {
                    currentElem?.SetAttribute(currentParameter, Table[(byte)instr.Value]);
                    break;
                }
                case Instruction.NewValueTable:
                {
                    byte id = (byte)instr.Value;
                    string value = id.ToString();
                    if (id < ValueTable.Count)
                        value = ValueTable[id];
                    currentElem?.SetAttribute(currentParameter, value);
                    break;
                }
                case Instruction.NewValueBool:
                {
                    currentElem?.SetAttribute(currentParameter, (byte)instr.Value != 0 ? "true" : "false");
                    break;
                }
                case Instruction.NewValueInt:
                {
                    currentElem?.SetAttribute(currentParameter, ((int)instr.Value).ToString());
                    break;
                }
                case Instruction.NewValueUInt:
                {
                    currentElem?.SetAttribute(currentParameter, ((uint)instr.Value).ToString());
                    break;
                }
                case Instruction.NewValueFloat:
                {
                    currentElem?.SetAttribute(currentParameter, ((float)instr.Value).ToString());
                    break;
                }
                case Instruction.NewNode:
                {
                    byte id = (byte)instr.Value;
                    parentElem = currentElem;
                    currentElem = doc.CreateElement(Table[id]);
                    if (parentElem != null)
                        parentElem.AppendChild(currentElem);
                    else
                        doc.AppendChild(currentElem);
                    break;
                }
                case Instruction.NewNodeTable:
                {
                    byte id = (byte)instr.Value;
                    parentElem = currentElem;
                    currentElem = doc.CreateElement(NodeIDToString(IDTable, id));
                    if (parentElem != null)
                        parentElem.AppendChild(currentElem);
                    else
                        doc.AppendChild(currentElem);
                    break;
                }


            }
        }
        return doc;
    }
    public void Write(BinaryObjectWriter writer)
    {

    }
}