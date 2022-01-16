using Microsoft.Extensions.Options;
using NasaService.Models;
using System.Text.Json;

namespace NasaService
{
    /// <summary>
    /// Class containing worker execution logic.
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private IHostApplicationLifetime _appLifetime { get; set; }

        private IImageGetter _imageGetter { get; set; }

        private CoreOptions CoreOptions { get; set; }

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime appLifetime,
            IImageGetter imageGetter,
            IOptions<CoreOptions> options)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _imageGetter = imageGetter;
            
            CoreOptions = options.Value;
        }

        /// <summary>
        /// Gets NASA images by date.
        /// </summary>
        /// <param name="stoppingToken">Token indicating when execution should stop.</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // Read all dates.
            string[] dateFile = await File.ReadAllLinesAsync("dates.txt", stoppingToken);

            // Iterate through each date.
            foreach(string line in dateFile)
            {
                // Process only valid dates.
                if (DateOnly.TryParse(line, out DateOnly imageDate))
                {
                    _logger.LogInformation(imageDate.ToString());
                    await _imageGetter.GetImages(imageDate, CoreOptions.ImageSaveLocation, stoppingToken);
                }
            }

            _logger.LogInformation("Finished getting images. Now just spinning until a termination signal is sent.");

            // This is just a spin wait for Docker debugging purposes.
            while (!stoppingToken.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                _logger.LogInformation("Spin.");
            }

            _appLifetime.StopApplication();
        }
    }
}