namespace SharpNeedle.Framework.BIXF;

public enum Instruction : byte
{
    GoToParent = 0x0,
    NewNode = 0x21,
    NewParameter = 0x41,
    NewValue = 0x61,
    NewNodeTable = 0x29,
    NewParameterTable = 0x49,
    NewValueTable = 0x69,
    NewValueBool = 0x70,
    NewValueInt = 0x74,
    NewValueUInt = 0x75,
    NewValueFloat = 0x76
}
