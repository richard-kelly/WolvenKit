namespace WolvenKit.RED4.Types;

public class DynamicBaseClass : RedBaseClass, IDynamicClass
{
    [REDProperty(IsIgnored = true)]
    public string ClassName { get; set; }

    [REDProperty(IsIgnored = true)]
    public bool IsResource { get; set; }
}

public class DynamicResource : CResource, IDynamicClass
{
    [REDProperty(IsIgnored = true)]
    public string ClassName { get; set; }

    [REDProperty(IsIgnored = true)]
    public bool IsResource { get; set; }
}
