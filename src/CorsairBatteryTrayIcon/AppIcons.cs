using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CorsairBatteryTrayIcon;

public class AppIcons
{
    public string IconDir { get; }
    public Icon Default { get; }
    public Icon Charging { get; }
    public Icon Unknown { get; set; }

    private readonly BatteryIcon[] _batteryIcons;

    public AppIcons(
        string iconDir
    )
    {
        ValidateIconDir(iconDir);
        IconDir = iconDir;
        Default = LoadIconByName("default");
        Charging = LoadIconByName("charging");
        Unknown = LoadIconByName("unknown");
        _batteryIcons = LoadBatteryIcons();
    }

    private void ValidateIconDir(
        string iconDir
    )
    {
        if (Directory.Exists(iconDir))
        {
            return;
        }

        if (File.Exists(iconDir))
        {
            throw new InvalidOperationException($"File exists at '{iconDir}' - please remove it");
        }

        Directory.CreateDirectory(iconDir);
    }

    private BatteryIcon[] LoadBatteryIcons()
    {
        var localIcons = Directory.GetFiles(IconDir)
            .Where(FileNameIsNumeric)
            .Select(GenerateBatteryIconFromLocalFile)
            .OrderByDescending(icon => icon.MinPercent)
            .ToArray();
        if (localIcons.Any())
        {
            return localIcons;
        }

        var asm = typeof(AppIcons).Assembly;
        return asm.GetManifestResourceNames()
            .Select(name => name.Split('.').Reverse().Skip(1).Reverse().LastOrDefault())
            .Where(FileNameIsNumeric)
            .Select(GenerateBatteryIconFromResource)
            .OrderByDescending(icon => icon.MinPercent)
            .ToArray();
    }

    private bool FileNameIsNumeric(
        string filePath
    )
    {
        if (filePath is null)
        {
            return false;
        }

        var noExtension = DetermineFileNameWithoutExtension(filePath);
        return int.TryParse(
            noExtension,
            out _
        );
    }

    private BatteryIcon GenerateBatteryIconFromResource(
        string resource
    )
    {
        var icon = LoadIconByName(resource);
        return GenerateBatteryIcon(
            resource,
            icon
        );
    }

    private BatteryIcon GenerateBatteryIconFromLocalFile(
        string pathToIcon
    )
    {
        var icon = LoadIconByPath(pathToIcon);
        return GenerateBatteryIcon(
            pathToIcon,
            icon
        );
    }

    private BatteryIcon GenerateBatteryIcon(
        string name,
        Icon icon
    )
    {
        var percentageString = DetermineFileNameWithoutExtension(name);
        var percent = int.Parse(percentageString);
        return new BatteryIcon(
            icon,
            percent
        );
    }

    private string DetermineFileNameWithoutExtension(
        string filePath
    )
    {
        if (_fileNameCache.TryGetValue(
                filePath,
                out var cached
            ))
        {
            return cached;
        }

        var fileName = Path.GetFileName(filePath);
        var result = Path.GetFileNameWithoutExtension(fileName);
        _fileNameCache[filePath] = result;
        return result;
    }

    private readonly Dictionary<string, string> _fileNameCache = new();

    private Icon LoadDefaultIcon()
    {
        return LoadIconByName("default");
    }

    private Icon LoadIconByName(
        string name
    )
    {
        var fileName = $"{name}.ico";
        var fullPath = Path.Combine(
            IconDir,
            fileName
        );
        return File.Exists(fullPath)
            ? LoadIconByPath(fullPath)
            : TryLoadResourceIcon(fileName);
    }

    private static Icon LoadIconByPath(
        string fullPath
    )
    {
        using var stream = File.Open(
            fullPath,
            FileMode.Open
        );
        return new Icon(
            stream
        );
    }

    private static Icon TryLoadResourceIcon(
        string fullPath
    )
    {
        var icon = Path.GetFileName(fullPath);
        var asm = typeof(AppIcons).Assembly;
        var resourceName = asm.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(icon))
            ?? throw new InvalidOperationException(
                $"'{icon}' not found in icons folder and no default embedded resource!"
            );
        using var stream = asm.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"unable to load resource stream '{resourceName}");
        return new Icon(stream);
    }

    public Icon BatteryIconForPercent(
        int batteryPercent
    )
    {
        if (batteryPercent < 0)
        {
            return _batteryIcons.LastOrDefault()?.Icon ?? Default;
        }

        if (batteryPercent == _lastBatteryPercent &&
            _lastBatteryIcon is not null)
        {
            return _lastBatteryIcon;
        }

        var deltas = _batteryIcons.Select(
            icon => new { delta = Math.Abs(batteryPercent - icon.MinPercent), icon }
        ).ToArray();
        var closest = deltas.Min(o => o.delta);
        var match = deltas.FirstOrDefault(o => o.delta == closest);
        if (match is null)
        {
            _lastBatteryIcon = null;
            return Default;
        }

        _lastBatteryPercent = batteryPercent;
        _lastBatteryIcon = match.icon.Icon;
        return _lastBatteryIcon;
    }

    private object _iconLock = new object();
    private int _lastBatteryPercent = int.MinValue;
    private Icon _lastBatteryIcon = null;
}