using NasaService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.AddUserSecrets<Program>().Build();
    })
    .ConfigureServices((host, services) =>
    {
        services.AddHostedService<Worker>();

        // Get configuration sections from appsettings.json.
        NasaWebApiOptions webApiOptions = new();
        NasaFileOptions fileOptions = new();

        var fileConfigSection = host.Configuration.GetSection("NasaFileOptions");
        var webApiConfigSection = host.Configuration.GetSection("NasaWebApiOptions");

        fileConfigSection.Bind(fileOptions);
        webApiConfigSection.Bind(webApiOptions);
        
        // If there is a file path specified in the appsettings.json, we'll use this to ingest a downloaded
        // json response we've already received from nasa.gov. This is essentially for testing without having
        // to cost us an API credit.
        if (!string.IsNullOrEmpty(fileOptions.FilePath))
        {
            services.AddSingleton<INasaReplyReader, NasaReplyFileReader>()
                .Configure<NasaFileOptions>(fileConfigSection);
        }
        // If the FilePath parameter is blank, assume standard API usage.
        else
        {
            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient();

            services.AddSingleton<INasaReplyReader, NasaReplyWebReader>()
                .Configure<NasaWebApiOptions>(webApiConfigSection);
        }

        // The NasaImageGetter will make use of either the NasaReplyFileReader, or the NasaReplyWebReader -- whichever
        // is implied via the appsettings.json.
        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
