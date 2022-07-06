using System;

namespace WolvenKit.RED4.Types;

public enum HandlerResult
{
    NotHandled,
    Modified,
    Ignore,
    Skip
}

public abstract class ParsingErrorEventArgs : EventArgs { }

public delegate HandlerResult ParsingErrorEventHandler(ParsingErrorEventArgs e);

public class InvalidRTTIEventArgs : ParsingErrorEventArgs
{
    public string PropertyName { get; set; }
    public Type ExpectedType { get; }
    public Type ActualType { get; }
    public IRedType Value { get; set; }

    public InvalidRTTIEventArgs(string propertyName, Type expectedType, Type actualType, IRedType value)
    {
        PropertyName = propertyName;
        ExpectedType = expectedType;
        ActualType = actualType;
        Value = value;
    }
}

public class InvalidDefaultValueEventArgs : ParsingErrorEventArgs
{

}

public class UnknownRTTIEventArgs : ParsingErrorEventArgs
{
    public RedTypeInfo RedTypeInfo { get; }

    public UnknownRTTIEventArgs(RedTypeInfo redTypeInfo)
    {
        RedTypeInfo = redTypeInfo;
    }
}

public class UnknownPropertyEventArgs : ParsingErrorEventArgs
{
    public string PropertyName { get; set; }

    public UnknownPropertyEventArgs(string propertyName)
    {
        PropertyName = propertyName;
    }
}
