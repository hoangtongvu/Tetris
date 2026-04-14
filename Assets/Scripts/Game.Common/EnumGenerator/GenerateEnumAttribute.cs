using System;

namespace Game.Common.EnumGenerator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class GenerateEnumAttribute : Attribute
{
    public GenerateEnumAttribute(string enumNamespace, string enumName, string memberName, int explicitMemeberValue = -1)
    {
    }
}