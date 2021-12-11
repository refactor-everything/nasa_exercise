namespace NasaService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private string ApiKey { get; set; }
        private string ApiUrl { get; set; }

        private IHostApplicationLifetime AppLifetime { get; set; }

        private IImageGetter ImageGetter { get; set; }

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IHostApplicationLifetime appLifetime, IImageGetter imageGetter)
        {
            _logger = logger;
            ApiUrl = configuration["ApiUrl"];
            ApiKey = configuration["ApiKey"];
            AppLifetime = appLifetime;
            ImageGetter = imageGetter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            ImageGetter.ApiUrl = ApiUrl;
            ImageGetter.ApiKey = ApiKey;

            DateOnly imageDate = new(2015, 06, 03);

            var reply = await ImageGetter.GetImageUrls(imageDate);

            AppLifetime.StopApplication();
        }
    }
}