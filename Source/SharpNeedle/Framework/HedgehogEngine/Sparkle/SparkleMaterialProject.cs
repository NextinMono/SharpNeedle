namespace SharpNeedle.Framework.HedgehogEngine.Sparkle;

[NeedleResource("hh/sparkle", @"\.p-mat-bin$")]
public class SparkleMaterialProject : ResourceBase, IBinarySerializable
{
    public ProjectInfo? ProjectInfo { get; set; }
    public Material? Material { get; set; }

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

        using BinaryObjectWriter writer = new(file.Open(FileAccess.Write), StreamOwnership.Transfer, Endianness.Little);
        Write(writer);
    }
    public void Read(BinaryObjectReader reader)
    {
        ProjectInfo = reader.ReadObject<ProjectInfo>();

        //InportExportMaterial
        Material = reader.ReadObject<Material>();
    }
    public void Write(BinaryObjectWriter writer)
    {
        writer.WriteObject(ProjectInfo);
        writer.WriteObject(Material);

        //S E G A
        writer.Write(83);
        writer.Write(69);
        writer.Write(71);
        writer.Write(65);
    }
}