using NasaService;

string ApiUrl = "";
string ApiKey = "";
string FilePath = "";

IConfiguration Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        Configuration = app.AddUserSecrets<Program>()
        .Build();

        ApiUrl = Configuration["ApiUrl"];
        ApiKey = Configuration["ApiKey"];
        FilePath = Configuration["FilePath"];
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        if (!string.IsNullOrEmpty(FilePath))
        {
            services.AddSingleton<INasaReplyReader, NasaReplyFileReader>(
                factory => new NasaReplyFileReader(FilePath));
        }
        else
        {
            services.AddSingleton<INasaReplyReader, NasaReplyWebReader>(
                factory => new NasaReplyWebReader(ApiUrl, ApiKey));

            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient<INasaReplyReader, NasaReplyWebReader>();
        }

        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
