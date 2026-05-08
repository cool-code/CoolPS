using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Cool;

public class ConfigLoader
{
    private static readonly Dictionary<string, string> _translateMap = new(StringComparer.OrdinalIgnoreCase) {
        {"BLK", "bd"},
        {"CAPABILITY", "ca"},
        {"CHR", "cd"},
        {"DIR", "di"},
        {"DOOR", "do"},
        {"EXEC", "ex"},
        {"FIFO", "pi"},
        {"FILE", "fi"},
        {"HIDDEN", "hi"},
        {"LINK", "ln"},
        {"MISSING", "mi"},
        {"MULTIHARDLINK", "mh"},
        {"NORMAL", "no"},
        {"ORPHAN", "or"},
        {"OTHER_WRITABLE", "ow"},
        {"RESET", "rs"},
        {"SETGID", "sg"},
        {"SETUID", "su"},
        {"SOCK", "so"},
        {"STICKY", "st"},
        {"STICKY_OTHER_WRITABLE", "tw"}
    };
    private static readonly Regex _splitRegex = new(@"\s+(?=\S+$)", RegexOptions.Compiled);
    private static string ConvertFromSourceData(string sourceFile)
    {
        if (!File.Exists(sourceFile)) return string.Empty;

        var filters = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "TERM", "COLOR", "*" };
        var result = new StringBuilder(16384);

        foreach (string line in File.ReadLines(sourceFile))
        {
            // remove comments and trim whitespace
            string cleanLine = line.Split('#')[0].Trim();
            if (string.IsNullOrEmpty(cleanLine)) continue;

            // use regex to split by last occurrence of whitespace, so we can support values with spaces (like "Saved Games")
            string[] parts = _splitRegex.Split(cleanLine);
            if (parts.Length < 2) continue;

            string key = parts[0];
            string val = parts[1];

            if ((val == "*") || (val == "") || filters.Contains(key)) continue;

            string finalKey;
            if (_translateMap.TryGetValue(key, out string translated))
            {
                finalKey = translated;
            }
            else if (key.StartsWith("*."))
            {
                finalKey = key.ToLower();
            }
            else if (key.StartsWith("."))
            {
                finalKey = "*" + key.ToLower();
            }
            else
            {
                finalKey = key;
            }

            if (filters.Contains(finalKey)) continue;

            filters.Add(key);
            filters.Add(finalKey);
            if (result.Length > 0) result.Append(':');
            result.Append(finalKey).Append('=').Append(val);
        }
        return result.ToString();
    }

    private static string GetCacheData(string sourceFile, string cacheFile)
    {
        if (File.Exists(cacheFile))
        {
            string cached = File.ReadAllText(cacheFile, Encoding.UTF8);
            if (!string.IsNullOrEmpty(cached)) return cached;
        }
        string data = ConvertFromSourceData(sourceFile);
        File.WriteAllText(cacheFile, data, Encoding.UTF8);
        return data;
    }
    public static readonly Dictionary<string, string> DefaultColors = new()
    {
        // For file system objects
        { "fi", Color.Default }, // File
        { "di", Color.Turquoise }, // Directory
        { "ln", Color.Magenta }, // Symbolic link
        { "or", Color.Red }, // Orphan symbolic link
        { "ex", Color.Green }, // Executable
        { "hi", Color.Maroon }, // Hidden
        { "pi", Color.Purple }, // Pipe
        { "so", Color.Silver }, // Socket
        { "bd", Color.Blue }, // Block device
        { "cd", Color.Cyan }, // Character device

        // For Command types
        { "Alias", Color.Magenta },
        { "Function", Color.Cyan },
        { "Filter", Color.Yellow },
        { "Cmdlet", Color.Blue },
        { "ExternalScript", Color.Blue },
        { "Application", Color.Green },
        { "Script", Color.Teal },
        { "Workflow", Color.Purple },
        { "Configuration", Color.Gray },
        { "Drive", Color.Red },

        // For Completion result types
        { "History", Color.Yellow },
        { "Property", Color.Cyan },
        { "Method", Color.Blue },
        { "ParameterName", Color.Orange },
        { "ParameterValue", Color.Green },
        { "Variable", Color.Teal },
        { "Namespace", Color.Turquoise },
        { "Type", Color.Turquoise },
        { "Keyword", Color.Red },
        { "DynamicKeyword", Color.Maroon },
        { "Text", Color.White },
    };
    public static readonly Dictionary<string, string> DefaultIcons = new()
    {
        // For file system objects
        { "fi", "" }, // File
        { "di", "" }, // Directory
        { "ln", "" }, // Symbolic link
        { "or", "" }, // Orphan symbolic link
        { "ex", "" }, // Executable
        { "hi", "" }, // Hidden
        { "pi", "" }, // Pipe
        { "so", "" }, // Socket
        { "bd", "" }, // Block device
        { "cd", "" }, // Character device

        // For Command types
        { "Alias", "" },
        { "Function", "" },
        { "Filter", "" },
        { "Cmdlet", "" },
        { "ExternalScript", "" },
        { "Application", "" },
        { "Script", "" },
        { "Workflow", "" },
        { "Configuration", "" },
        { "Drive", "" },

        // For Completion result types
        { "History", "" },
        { "Property", "" },
        { "Method", "" },
        { "ParameterName", "" },
        { "ParameterValue", "" },
        { "Variable", "" },
        { "Namespace", "" },
        { "Type", "" },
        { "Keyword", "" },
        { "DynamicKeyword", "" },
        { "Text", "" },
    };
    private static readonly Regex _extractionRegex = new(@"^(.*)\[0-9\]\{0,(\d+)\}$", RegexOptions.Compiled);
    private static Dictionary<string, string> ConvertToMemCache(string envVar, string envVarName)
    {
        Dictionary<string, string> hash = envVarName switch
        {
            "LS_COLORS" => new Dictionary<string, string>(DefaultColors, StringComparer.OrdinalIgnoreCase),
            "LS_ICONS" => new Dictionary<string, string>(DefaultIcons, StringComparer.OrdinalIgnoreCase),
            _ => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
        };

        if (string.IsNullOrEmpty(envVar)) return hash;

        foreach (var item in envVar.Split(':'))
        {
            var kv = item.Split('=');
            if (kv.Length != 2) continue;

            string key = kv[0];
            string val = (envVarName == "LS_COLORS") ? Ansi.EscapeSGR(kv[1]) : kv[1];

            // extraction logic:
            // if key matches "PREFIX[0-9]{0,3}", extract "PREFIX" and "3",
            // then generate keys like "PREFIX", "PREFIX0", "PREFIX00", ..., "PREFIX999" (up to 3 digits)
            var match = _extractionRegex.Match(key);
            if (match.Success)
            {
                string prefix = match.Groups[1].Value.TrimStart('*');
                int maxLen = int.Parse(match.Groups[2].Value);
                // limit maxLen to 3 to avoid generating too many keys
                if (maxLen > 3) maxLen = 3;

                hash[prefix] = val;
                int maxN = (int)Math.Pow(10, maxLen) - 1;
                for (int i = 0; i <= maxN; i++)
                {
                    string n = i.ToString();
                    for (int j = n.Length; j <= maxLen; j++)
                    {
                        hash[prefix + n.PadLeft(j, '0')] = val;
                    }
                }
            }
            else if (key.StartsWith("*"))
            {
                hash[key.TrimStart('*')] = val;
            }
            else
            {
                hash[key] = val;
            }
        }
        return hash;
    }

    private static Dictionary<string, string> Load(string envName, string sourceFile, string cacheFile, bool force = false)
    {
        if (force) { try { File.Delete(cacheFile); } catch { /* ignore */ } }

        var data = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrEmpty(data) || force)
        {
            data = GetCacheData(sourceFile, cacheFile);
            Environment.SetEnvironmentVariable(envName, data);
        }

        return ConvertToMemCache(data, envName);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public static Dictionary<string, string> LoadColors(string sourceFile, string cacheFile, bool force = false)
    {
        return Load("LS_COLORS", sourceFile, cacheFile, force);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public static Dictionary<string, string> LoadIcons(string sourceFile, string cacheFile, bool force = false)
    {
        return Load("LS_ICONS", sourceFile, cacheFile, force);
    }

}