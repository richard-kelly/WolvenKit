using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// ReSharper disable InconsistentNaming

namespace WolvenKit.RED4.Types;

public class ExtendedEnumInfo
{
    public Type Type { get; set; }
    public bool IsBitfield { get; set; }

    public Dictionary<string, string> RedNames { get; set; } = new();

    public ExtendedEnumInfo(Type type)
    {
        Type = type;
        IsBitfield = type.GetCustomAttribute<FlagsAttribute>() != null;

        var valueNames = Enum.GetNames(Type);
        foreach (var valueName in valueNames)
        {
            var member = Type.GetMember(valueName);

            var redAttr = member[0].GetCustomAttribute<REDAttribute>();
            RedNames.Add(redAttr != null ? redAttr.Name : valueName, valueName);
        }
    }

    public string GetCSNameFromRedName(string valueName)
    {
        if (RedNames.ContainsKey(valueName))
        {
            return RedNames[valueName];
        }

        return null;
    }

    public string GetRedNameFromCSName(string valueName)
    {
        if (RedNames.ContainsValue(valueName))
        {
            return RedNames.FirstOrDefault(x => x.Value == valueName).Key;
        }

        return valueName;
    }
}
