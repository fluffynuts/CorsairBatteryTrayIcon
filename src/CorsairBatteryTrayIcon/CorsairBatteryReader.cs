using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HidApiAdapter;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CorsairBatteryTrayIcon;

class CorsairBatteryReader
{
    public EventHandler<BatteryStatusEventArgs>? OnBatteryStatusUpdate { get; set; }

    private const int MIC_UP_OFFSET = 128;
    public const int VENDOR_ID = 0x1b1c;
    public int ProductId = 0x0a14;
    private byte[] _dataReq = { 0xC9, 0x64 };

    private int?[] LastValues { get; set; }
    private const int FilterLength = 25;

    private IntPtr _devPtr;
    private bool _backgroundScanningEnabled;

    public CorsairBatteryReader()
    {
        LastValues = new int?[FilterLength];
    }

    private int FilterValue(int value)
    {
        int sum = 0, i;

        if (!LastValues.Contains(null))
        {
            // shift array left
            var newArray = new int?[FilterLength];
            Array.Copy(LastValues, 1, newArray, 0, LastValues.Length - 1);
            LastValues = newArray;
        }

        for (i = 0; i < LastValues.Length; i++)
        {
            if (LastValues[i] == null)
            {
                LastValues[i] = value;
                sum += value;
                break;
            }
            else
            {
                var lastValueAtI = LastValues[i];
                if (lastValueAtI is not null)
                {
                    sum += (int)lastValueAtI;
                }
            }
        }

        return sum / (i + 1);
    }

    public void StartBackgroundScanning()
    {
        _backgroundScanningEnabled = true;
        Task.Run(() =>
        {
            while (_backgroundScanningEnabled)
            {
                var buffer = GetBatteryStatusViaHid();
                TraceWrite(buffer);
                if (buffer is null)
                {
                    OnBatteryStatusUpdate?.Invoke(this, new DeviceNotFoundEventArgs());
                    SleepWhilstScanning(1000);
                }
                else
                {
                    HandleReport(buffer);
                    SleepWhilstScanning(5000);
                }
            }
        });
    }

    private void TraceWrite(byte[]? buffer)
    {
        if (buffer is null)
        {
            Trace.WriteLine("retrieved buffer was null");
            return;
        }

        Trace.WriteLine($"retrieved: [{string.Join(", ", buffer)}]");
    }

    public void StopBackgroundScanning()
    {
        _backgroundScanningEnabled = false;
    }

    private void SleepWhilstScanning(int ms)
    {
        var end = DateTime.Now.AddMilliseconds(ms);
        while (DateTime.Now < end && _backgroundScanningEnabled)
        {
            Thread.Sleep(100);
        }
    }


    private HidDevice? GetHidDevice()
    {
        var devices = HidDeviceManager.GetManager().SearchDevices(VENDOR_ID, ProductId);

        foreach (var dev in devices)
        {
            if (dev.Path().Contains("col02"))
            {
                return dev;
            }
        }

        if (devices.Count > 0)
        {
            return devices.FirstOrDefault();
        }

        return null;
    }

    private byte[]? GetBatteryStatusViaHid()
    {
        var device = GetHidDevice();
        if (device is null)
        {
            return null;
        }

        device.Connect();

        //get handle via reflection, because its a private field (oof)
        var field = typeof(HidDevice).GetField("m_DevicePtr",
            BindingFlags.NonPublic | BindingFlags.Instance);
        _devPtr = (IntPtr)field.GetValue(device);

        var buffer = new byte[5];
        HidApi.hid_write(_devPtr, _dataReq, Convert.ToUInt32(_dataReq.Length));
        HidApi.hid_read_timeout(_devPtr, buffer, Convert.ToUInt32(buffer.Length), 1000);
        device.Disconnect();
        Thread.Sleep(250);
        return buffer;
    }

    private static readonly HashSet<int> ChargingValues = new(new[] { 0, 4, 5 });

    private void HandleReport(byte[] data)
    {
        try
        {
            var batteryPercent = data[2];
            if (batteryPercent > MIC_UP_OFFSET)
            {
                batteryPercent -= MIC_UP_OFFSET;
            }

            var state = ChargeStates.Unknown;
            switch (data[4])
            {
                case 0:
                    state = ChargeStates.Disconnected;
                    break;
                case 4:
                case 5:
                    state = ChargeStates.Charging;
                    break;
                default:
                    state = ChargeStates.Discharging;
                    break;
            }

            var args = new BatteryStatusEventArgs(
                batteryPercent,
                state
            );
            var handlers = OnBatteryStatusUpdate;
            handlers?.Invoke(this, args);
        }
        catch
        {
            return;
        }
    }
}