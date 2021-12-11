using NasaService.Models;
using System.Text.Json;

namespace NasaService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> Logger;

        private IHostApplicationLifetime AppLifetime { get; set; }

        private IImageGetter ImageGetter { get; set; }

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime appLifetime,
            IImageGetter imageGetter)
        {
            Logger = logger;
            AppLifetime = appLifetime;
            ImageGetter = imageGetter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            string[] dateFile = await File.ReadAllLinesAsync("dates.txt", stoppingToken);

            foreach(string line in dateFile)
            {
                if (DateOnly.TryParse(line, out DateOnly imageDate))
                {
                    Logger.LogInformation(imageDate.ToString());
                    await ImageGetter.GetImages(imageDate, @"C:\image_download");
                }
            }

            AppLifetime.StopApplication();
        }
    }
}