using System.Drawing;

namespace CorsairBatteryTrayIcon;

public class BatteryIcon
{
    public Icon Icon { get; }
    public int MinPercent { get; }

    public BatteryIcon(
        Icon icon,
        int minPercent
    )
    {
        Icon = icon;
        MinPercent = minPercent;
    }
}