using NasaService.Models;
using System.Text.Json;

namespace NasaService
{
    /// <summary>
    /// Class containing worker execution logic.
    /// </summary>
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

        /// <summary>
        /// Gets NASA images by date.
        /// </summary>
        /// <param name="stoppingToken">Token indicating when execution should stop.</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // Read all dates.
            string[] dateFile = await File.ReadAllLinesAsync("dates.txt", stoppingToken);

            // Iterate through each date.
            foreach(string line in dateFile)
            {
                // Process only valid dates.
                if (DateOnly.TryParse(line, out DateOnly imageDate))
                {
                    Logger.LogInformation(imageDate.ToString());
                    await ImageGetter.GetImages(imageDate, @"C:\image_download", stoppingToken);
                }
            }

            AppLifetime.StopApplication();
        }
    }
}