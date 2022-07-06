namespace WolvenKit.RED4.Types;

public partial class DeviceTimetable
{
    [RED("timeTable")]
    public CArray<SDeviceTimetableEntry> TimeTable
    {
        get => GetPropertyValue<CArray<SDeviceTimetableEntry>>();
        set => SetPropertyValue<CArray<SDeviceTimetableEntry>>(value);
    }
}
