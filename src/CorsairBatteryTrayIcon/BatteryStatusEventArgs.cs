using System;

namespace CorsairBatteryTrayIcon;

public enum ChargeStates
{
    Unknown,
    Disconnected,
    Discharging,
    Charging
}

public class BatteryStatusEventArgs : EventArgs
{
    public int BatteryPercent { get; }
    public ChargeStates ChargeState { get; }

    public BatteryStatusEventArgs(
        int batteryPercent,
        ChargeStates chargeState
    )
    {
        BatteryPercent = batteryPercent;
        ChargeState = chargeState;
    }
}

public class DeviceNotFoundEventArgs : BatteryStatusEventArgs
{
    public DeviceNotFoundEventArgs() : base(
        0, ChargeStates.Unknown
    )
    {
    }
}