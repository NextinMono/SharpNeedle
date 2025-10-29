namespace SharpNeedle.Framework.BIXF;

using Amicitia.IO.Binary;

public struct Node : IBinarySerializable
{
    public Instruction ID;
    public Union Value;
    public void Read(BinaryObjectReader reader)
    {
        ID = (Instruction)reader.Read<byte>();
        switch (ID)
        {
            case Instruction.GoToParent:
            {
                break;
            }
            case Instruction.NewNodeTable:
            case Instruction.NewNode:
            case Instruction.NewValueBool:
            case Instruction.NewValueTable:
            case Instruction.NewValue:
            case Instruction.NewParameterTable:
            case Instruction.NewParameter:
            {
                Value.Set(reader.Read<byte>());
                break;
            }
            case Instruction.NewValueInt:
            {
                Value.Set(reader.Read<int>());
                break;
            }
            case Instruction.NewValueUInt:
            {
                Value.Set(reader.Read<uint>());
                break;
            }
            case Instruction.NewValueFloat:
            {
                Value.Set(reader.Read<float>());
                break;
            }
        }
    }

    public void Write(BinaryObjectWriter writer)
    {
        throw new NotImplementedException();
    }
}
