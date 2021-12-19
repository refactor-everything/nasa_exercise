using NasaService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.AddUserSecrets<Program>().Build();
    })
    .ConfigureServices((host, services) =>
    {
        services.AddHostedService<Worker>();

        NasaWebApiOptions webApiOptions = new();
        NasaFileOptions fileOptions = new();

        var fileConfigSection = host.Configuration.GetSection("NasaFileOptions");
        var webApiConfigSection = host.Configuration.GetSection("NasaWebApiOptions");

        fileConfigSection.Bind(fileOptions);
        webApiConfigSection.Bind(webApiOptions);

        if (!string.IsNullOrEmpty(fileOptions.FilePath))
        {
            services.AddSingleton<INasaReplyReader, NasaReplyFileReader>()
                .Configure<NasaFileOptions>(fileConfigSection);
        }
        else
        {
            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient();

            services.AddSingleton<INasaReplyReader, NasaReplyWebReader>()
                .Configure<NasaWebApiOptions>(webApiConfigSection);
        }

        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
