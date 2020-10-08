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
