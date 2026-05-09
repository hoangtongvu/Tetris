using System;

namespace Game.UI.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GenerateUITypeAttribute : Attribute
{
    public GenerateUITypeAttribute(string uiTypeName, uint underlyingValue)
    {
    }
}