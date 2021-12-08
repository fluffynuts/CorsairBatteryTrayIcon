using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PeanutButter.TrayIcon;

namespace CorsairBatteryTrayIcon;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var icons = new AppIcons(IconDir);
        var reader = new CorsairBatteryReader();
        var trayIcon = new TrayIcon(icons.Default);
        trayIcon.AddMenuItem("E&xit", () =>
        {
            reader.StopBackgroundScanning();
            trayIcon.Hide();
            Application.Exit();
        });
        trayIcon.Show();

        // TODO: load config: supported devices & selected device
        // -> for now, hard-coded to the HS70 pro
        reader.ProductId = 0x0A4F;
        reader.StartBackgroundScanning();
        reader.OnBatteryStatusUpdate += (_, args) =>
            UpdateTrayIconWithBatteryStatus(args, trayIcon, icons);
        Application.Run();
    }

    private static void UpdateTrayIconWithBatteryStatus(
        BatteryStatusEventArgs args,
        TrayIcon trayIcon,
        AppIcons appIcons
    )
    {
        trayIcon.Icon = IconForEvent(args, appIcons);
        trayIcon.NotifyIcon.Text = TextForEvent(args);
    }

    private static string TextForEvent(BatteryStatusEventArgs args)
    {
        switch (args.ChargeState)
        {
            case ChargeStates.Charging:
                return "Charging - unplug to check percent";
            case ChargeStates.Discharging:
                return $"Discharging - {args.BatteryPercent} %";
            case ChargeStates.Disconnected:
                return "Disconnected";
            default:
                return "Device not found or not recognised";
        }
    }

    private static Icon IconForEvent(
        BatteryStatusEventArgs args,
        AppIcons appIcons
    )
    {
        switch (args.ChargeState)
        {
            case ChargeStates.Charging:
                return appIcons.Charging;
            case ChargeStates.Discharging:
                return appIcons.BatteryIconForPercent(args.BatteryPercent);
            case ChargeStates.Disconnected:
                return appIcons.Disconnected;
            default:
                return appIcons.Default;
        }
    }

    private static string IconDir =>
        _iconDir ??= Path.Combine(AppDir, "icons");

    private static string? _iconDir;

    private static string AppDir =>
        _appDir ??= FindAppDir();

    private static string? _appDir;

    private static string FindAppDir()
    {
        var asmLocation = new Uri(typeof(Program).Assembly.Location).LocalPath;
        return Path.GetDirectoryName(asmLocation)
            ?? throw new InvalidOperationException("Unable to determine app folder");
    }
}