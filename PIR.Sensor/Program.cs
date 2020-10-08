using System;
using System.Device.Gpio;
using System.Threading;

namespace PIR.Sensor
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
