using Microsoft.Extensions.Configuration;
using SampleService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Service.Sample
{
    class Program
    {
        private static readonly string outFolderPath = "/var/log/SampleService";
        private static readonly string outFilePath = Path.Combine(outFolderPath, $"{DateTime.Now:yyyy-MM-dd}.txt");
        static void Main()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(CurrentAppSettings));
            var currentAppSettings = section.Get<CurrentAppSettings>();    

            if (!Directory.Exists(outFolderPath))
            {
                Directory.CreateDirectory(outFolderPath);
            }
            Console.WriteLine("Following settings applied during app startup:");
            Console.WriteLine($"Set app in constant loop: {currentAppSettings.IsEnabled}");
            Console.WriteLine($"App timeout: {currentAppSettings.Timeout}");
            Console.WriteLine($"Output file: {outFilePath}");
            Console.WriteLine();

            File.AppendAllLines(outFilePath, new List<string>() { "------------------- SERVICE START ------------------- " }, Encoding.UTF8);

            while (currentAppSettings.IsEnabled)
            {
                File.AppendAllLines(outFilePath, new List<string>() { DateTime.Now.ToString() }, Encoding.UTF8);
                Console.WriteLine(DateTime.Now);
                Thread.Sleep(Convert.ToInt32(currentAppSettings.Timeout));
            }
        }
    }
}