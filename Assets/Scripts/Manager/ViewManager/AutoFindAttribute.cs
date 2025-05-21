using System;

[AttributeUsage(AttributeTargets.Property)]
public class AutoFindAttribute : Attribute
{
    public string Value { get; }

    public AutoFindAttribute(string value = "", bool getOrAdd = true)
    {
        this.Value = value;
    }
}