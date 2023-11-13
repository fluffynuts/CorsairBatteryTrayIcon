using System;
using System.Diagnostics;
using System.Linq;
using HidApiAdapter;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CorsairBatteryTrayIcon;

class CorsairBatteryReader
{
    public EventHandler<BatteryStatusEventArgs> OnBatteryStatusUpdate { get; set; }

    private const int MIC_UP_OFFSET = 128;
    private const int VENDOR_ID = 0x1b1c;
    public int ProductId = 0x0a14;
    private readonly byte[] _dataReq = { 0xC9, 0x64 };

    private int?[] LastValues { get; set; }
    private const int FilterLength = 25;

    private IntPtr _devPtr;
    private bool _backgroundScanningEnabled;

    public CorsairBatteryReader()
    {
        LastValues = new int?[FilterLength];
    }

    public void StartBackgroundScanning()
    {
        _backgroundScanningEnabled = true;
        Task.Run(
            () =>
            {
                while (_backgroundScanningEnabled)
                {
                    var buffer = GetBatteryStatusViaHid();
                    TraceWrite(buffer);
                    if (buffer is null)
                    {
                        OnBatteryStatusUpdate?.Invoke(
                            this,
                            new DeviceNotFoundEventArgs()
                        );
                        SleepWhilstScanning(1000);
                    }
                    else
                    {
                        HandleReport(buffer);
                        SleepWhilstScanning(5000);
                    }
                }
            }
        );
    }

    private void TraceWrite(
        byte[] buffer
    )
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

    private void SleepWhilstScanning(
        int ms
    )
    {
        var end = DateTime.Now.AddMilliseconds(ms);
        while (DateTime.Now < end && _backgroundScanningEnabled)
        {
            Thread.Sleep(100);
        }
    }


    private HidDevice GetHidDevice()
    {
        var devices = HidDeviceManager.GetManager().SearchDevices(
            VENDOR_ID,
            ProductId
        );

        return devices.FirstOrDefault(
            dev => dev.Path().Contains("col02")
        ) ?? devices.FirstOrDefault();
    }

    private byte[] GetBatteryStatusViaHid()
    {
        var device = GetHidDevice();
        if (device is null)
        {
            return null;
        }

        device.Connect();

        //get handle via reflection, because its a private field (oof)
        var fieldName = "m_DevicePtr";
        var field = typeof(HidDevice).GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        if (field is null)
        {
            throw new InvalidOperationException(
                $"Attempted to read private field {fieldName}, but field was not found"
            );
        }

        _devPtr = (IntPtr) field.GetValue(device);

        var buffer = new byte[5];
        HidApi.hid_write(
            _devPtr,
            _dataReq,
            Convert.ToUInt32(_dataReq.Length)
        );
        HidApi.hid_read_timeout(
            _devPtr,
            buffer,
            Convert.ToUInt32(buffer.Length),
            1000
        );
        device.Disconnect();
        Thread.Sleep(250);
        return buffer;
    }

    private void HandleReport(
        byte[] data
    )
    {
        try
        {
            var batteryPercent = data[2];
            if (batteryPercent > MIC_UP_OFFSET)
            {
                batteryPercent -= MIC_UP_OFFSET;
            }

            var state = data[4] switch
            {
                0 => ChargeStates.Disconnected,
                4 => ChargeStates.Charging,
                5 => ChargeStates.Charging,
                _ => ChargeStates.Discharging
            };

            var args = new BatteryStatusEventArgs(
                batteryPercent,
                state
            );
            var handlers = OnBatteryStatusUpdate;
            handlers?.Invoke(
                this,
                args
            );
        }
        catch
        {
            // suppress
        }
    }
}