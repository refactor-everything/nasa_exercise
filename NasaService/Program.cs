using NasaService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        services.AddHostedService<Worker>()
            .Configure<CoreOptions>(host.Configuration.GetSection("CoreOptions"));

        // Get configuration sections from appsettings.json.
        NasaFileOptions fileOptions = host.Configuration
            .GetSection(nameof(NasaFileOptions))
            .Get<NasaFileOptions>();
        
        // If there is a file path specified in the appsettings.json, we'll use this to ingest a downloaded
        // json response we've already received from nasa.gov. This is essentially for testing without having
        // to cost us an API credit.
        if (!string.IsNullOrEmpty(fileOptions.FilePath))
        {
            services.AddSingleton<INasaReplyReader, NasaReplyFileReader>()
                .Configure<NasaFileOptions>(
                    host.Configuration.GetSection(nameof(NasaFileOptions)));
        }
        // If the FilePath parameter is blank, assume standard API usage.
        else
        {
            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<HttpClient>(string.Empty, config =>
            {
                config.DefaultRequestHeaders.Add("user-agent", ".NET Core 6.0 HttpClient");
            });

            services.AddSingleton<INasaReplyReader, NasaReplyWebReader>()
                .Configure<NasaWebApiOptions>(
                    host.Configuration.GetSection(nameof(NasaWebApiOptions)));
        }

        // The NasaImageGetter will make use of either the NasaReplyFileReader, or the NasaReplyWebReader -- whichever
        // is implied via the appsettings.json.
        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
