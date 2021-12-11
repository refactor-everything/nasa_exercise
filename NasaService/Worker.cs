using NasaService.Models;
using System.Text.Json;

namespace NasaService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private IHostApplicationLifetime AppLifetime { get; set; }

        private INasaReplyReader ReplyReader { get; set; }

        public Worker(
            ILogger<Worker> logger,
            IHostApplicationLifetime appLifetime,
            INasaReplyReader replyReader)
        {
            _logger = logger;
            AppLifetime = appLifetime;
            ReplyReader = replyReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            string[] dateFile = await File.ReadAllLinesAsync("dates.txt", stoppingToken);

            foreach(string line in dateFile)
            {
                if (DateOnly.TryParse(line, out DateOnly imageDate))
                {
                    _logger.LogInformation(imageDate.ToString());

                    string jsonReply = await ReplyReader.GetReply(imageDate);
                    NasaReply? nasaReply = JsonSerializer.Deserialize<NasaReply>(jsonReply);
                }
            }

            AppLifetime.StopApplication();
        }
    }
}