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
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            DateOnly imageDate = new(2015, 06, 03);

            string jsonReply = await ReplyReader.GetReply(imageDate);
            NasaReply? nasaReply = JsonSerializer.Deserialize<NasaReply>(jsonReply);

            AppLifetime.StopApplication();
        }
    }
}