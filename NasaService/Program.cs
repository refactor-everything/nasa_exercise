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
        }

        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
