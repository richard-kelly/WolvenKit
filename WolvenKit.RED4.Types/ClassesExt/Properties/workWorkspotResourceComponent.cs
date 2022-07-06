namespace WolvenKit.RED4.Types;

public partial class workWorkspotResourceComponent
{
    [RED("resource")]
    public CResourceReference<workWorkspotResource> Resource
    {
        get => GetPropertyValue<CResourceReference<workWorkspotResource>>();
        set => SetPropertyValue<CResourceReference<workWorkspotResource>>(value);
    }
}
