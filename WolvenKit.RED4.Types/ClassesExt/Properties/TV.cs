namespace WolvenKit.RED4.Types;

public partial class TV
{
    [RED("channels")]
    public CArray<STvChannel> Channels
    {
        get => GetPropertyValue<CArray<STvChannel>>();
        set => SetPropertyValue<CArray<STvChannel>>(value);
    }

    [RED("securedText")]
    public CString SecuredText
    {
        get => GetPropertyValue<CString>();
        set => SetPropertyValue<CString>(value);
    }

    [RED("muteInterface")]
    public CBool MuteInterface
    {
        get => GetPropertyValue<CBool>();
        set => SetPropertyValue<CBool>(value);
    }

    [RED("initialActiveChannel")]
    public CInt32 InitialActiveChannel
    {
        get => GetPropertyValue<CInt32>();
        set => SetPropertyValue<CInt32>(value);
    }
}
