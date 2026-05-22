using System.Collections.Generic;
using System.Management.Automation;
using System.Runtime.CompilerServices;

namespace Cool;

public static class Renderer
{
    private static volatile Dictionary<string, string> _colors = [];
    private static volatile Dictionary<string, string> _icons = [];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InitializeColors(string sourceFile, string cacheFile) => _colors = ConfigLoader.LoadColors(sourceFile, cacheFile);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InitializeIcons(string sourceFile, string cacheFile) => _icons = ConfigLoader.LoadIcons(sourceFile, cacheFile);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdateColors(string sourceFile, string cacheFile) => _colors = ConfigLoader.LoadColors(sourceFile, cacheFile, true);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UpdateIcons(string sourceFile, string cacheFile) => _icons = ConfigLoader.LoadIcons(sourceFile, cacheFile, true);

    private static string Lookup(string name, string ext, string attr, Dictionary<string, string> dict)
    {
        if (!string.IsNullOrEmpty(name) && dict.TryGetValue(name, out var value))
        {
            return value;
        }
        if (!string.IsNullOrEmpty(ext) && dict.TryGetValue(ext, out value))
        {
            return value;
        }
        if (!string.IsNullOrEmpty(attr) && dict.TryGetValue(attr, out value))
        {
            return value;
        }
        return string.Empty;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDefaultColor(string attr) => ConfigLoader.DefaultColors.TryGetValue(attr, out var color) ? color : string.Empty;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDefaultIcon(string attr) => ConfigLoader.DefaultIcons.TryGetValue(attr, out var icon) ? icon : string.Empty;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetColor(string name, string ext, string attr)
    {
        var color = Lookup(name, ext, attr, _colors);
        return (string.IsNullOrEmpty(color) || (color == "target")) ? GetDefaultColor(attr) : color;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetIcon(string name, string ext, string attr)
    {
        var icon = Lookup(name, ext, attr, _icons);
        return (string.IsNullOrEmpty(icon) || (icon == "target")) ? GetDefaultIcon(attr) : icon;
    }

    public static string FormatComandName(CommandInfo commandInfo)
    {
        var name = commandInfo.Name;
        var type = commandInfo.CommandType;
        string color, icon;

        if ((type & CommandTypes.Application) != 0)
        {
            var ext = System.IO.Path.GetExtension(name);
            color = GetColor(name, ext, "ex");
            icon = GetIcon(name, ext, "ex");
        }
        else
        {
            var typekey = type.ToString();
            switch (type)
            {
                case CommandTypes.Alias:
                    if (!string.IsNullOrEmpty(commandInfo.Definition))
                    {
                        name = $"{name} -> {commandInfo.Definition}";
                    }
                    break;
                case CommandTypes.Function:
                    if ((name.Length == 2) && name[0].IsBetween('A', 'Z') && (name[1] == ':'))
                    {
                        typekey = "Drive";
                    }
                    break;
            }
            color = GetDefaultColor(typekey);
            icon = GetDefaultIcon(typekey);
        }
        return string.Concat(color, icon, name, Ansi.Reset);
    }
}