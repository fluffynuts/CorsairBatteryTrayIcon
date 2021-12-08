using System;
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
        if (args.ChargeState == ChargeStates.Unknown)
        {
            trayIcon.NotifyIcon.Text = "Device not found or recognised";
            trayIcon.Icon = appIcons.Default;
            return;
        }

        trayIcon.NotifyIcon.Text = $"{args.BatteryPercent} %";
        trayIcon.Icon = args.ChargeState == ChargeStates.Discharging
            ? appIcons.BatteryIconForPercent(args.BatteryPercent)
            : appIcons.Charging;
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