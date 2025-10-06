﻿namespace SharpNeedle.Framework.HedgehogEngine.Sparkle;

[NeedleResource("hh/sparkle", @"\.part-bin$")]
public class SparkleParticleProject : ResourceBase, IBinarySerializable
{
    public ProjectInfo? ProjectInfo { get; set; }
    public Effect? Effect { get; set; }
    public List<Emitter> Emitters { get; set; } = [];

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

        //InportExportEffect
        Effect = reader.ReadObject<Effect>();
        for (int i = 0; i < ProjectInfo.EmitterCount; i++)
        {
            Emitters.Add(reader.ReadObject<Emitter>());
        }
    }
    public void Write(BinaryObjectWriter writer)
    {
        writer.WriteObject(ProjectInfo);

        writer.WriteObject(Effect);
        for (int i = 0; i < ProjectInfo.EmitterCount; i++)
        {
            writer.WriteObject(Emitters[i]);
        }

        //S E G A
        writer.Write(83);
        writer.Write(69);
        writer.Write(71);
        writer.Write(65);
    }
}