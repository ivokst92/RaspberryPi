# Raspberry Pi Utils

## Deploy console apps from windows to raspberry
Transfer files using WinSCP
Command: dotnet publish -c Release


## 1. PIR.Sensor
Useful links:
[PIR Sensor - HC-SR501](https://erelement.com/sensors/pir-sensor?cPath=9_28&zenid=rkqu13tqhksemgsg1tokf4its7)
[How to connect PIR to the pi](https://www.youtube.com/watch?v=Tw0mG4YtsZk)
[RaspPi PINS Image](https://www.raspberrypi-spy.co.uk/wp-content/uploads/2012/06/Raspberry-Pi-GPIO-Header-with-Photo.png)

Don't forget to set the right pin.
Nugets:
Iot.Device.Bindings (1.0.0)
```c#
using System;
using System.Device.Gpio;
using System.Threading;

namespace SensorPIR.ConsoleApp
{
    class Program
    {
        private const int PIR_PIN = 4;

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing GPIO");

            using (var gpio = new GpioController())
            using (var cts = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (s, e) => cts.Cancel();

                gpio.OpenPin(PIR_PIN, PinMode.Input);

                Console.WriteLine("Monitoring PIR sensor. ctrl+c to cancel.");
                bool lastOn = false;

                while (!cts.IsCancellationRequested)
                {
                    bool pirOn = gpio.Read(PIR_PIN) == true;

                    if (lastOn != pirOn)
                    {
                        Console.WriteLine($"Motion sensor is now {(pirOn ? "on" : "off")}");
                        lastOn = pirOn;
                    }
                }

                Console.WriteLine("Cleaning up");
                gpio.ClosePin(PIR_PIN);
            }
        }
    }
}

```

## 2. CameraCapture 
Camera - 5MP, Fisheye Lens,with IR leds, Night Vision - WaveShare RPi Camera (H)
Nugets:
Unosquare.Raspberry.IO (0.27.1)
```c#
using System;
using System.IO;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Camera;

namespace CaptureImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Say cheese!");
            CaptureImage();
            Console.WriteLine("Captured!");
        }

        static void CaptureImage()
        {
            var settings = new CameraStillSettings
            {
                CaptureExposure = CameraExposureMode.Night,
                CaptureWidth = 1920,
                CaptureHeight = 1080,
                CaptureJpegQuality = 100,
                CaptureTimeoutMilliseconds = 300
            };

            var pictureBytes = Pi.Camera.CaptureImage(settings);
            var targetPath = $"/home/pi/{Guid.NewGuid()}.jpg";
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            File.WriteAllBytes(targetPath, pictureBytes);
            Console.WriteLine($"Took picture -- Byte count: {pictureBytes.Length}");
        }
    }
}

```

### Used Pi - Raspberry Pi 4 Model B
