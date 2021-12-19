using NasaService;

//string ApiUrl = "";
//string ApiKey = "";
//string FilePath = "";

//IConfiguration Configuration;

//NasaWebApiOptions NasaWebApiOptions = new();
//NasaFileOptions NasaFileOptions = new();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.AddUserSecrets<Program>().Build();
        //Configuration = app.AddUserSecrets<Program>()
        //.Build();

        //ApiUrl = Configuration["ApiUrl"];
        //ApiKey = Configuration["ApiKey"];
        //FilePath = Configuration["FilePath"];

        
        
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
            //services.AddSingleton<INasaReplyReader, NasaReplyFileReader>(
            //    factory => new NasaReplyFileReader(FilePath));

            services.AddSingleton<INasaReplyReader, NasaReplyFileReader>()
                .Configure<NasaFileOptions>(fileConfigSection);
        }
        else
        {
            //services.AddSingleton<INasaReplyReader, NasaReplyWebReader>(
            //    factory => new NasaReplyWebReader(ApiUrl, ApiKey));

            //services.Configure<NasaWebApiOptions>(host.Configuration.GetSection("NasaWebApiOptions"));

            services.AddSingleton<INasaReplyReader, NasaReplyWebReader>()
                .Configure<NasaWebApiOptions>(webApiConfigSection)
                .AddHttpClient();

            // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            //services.AddHttpClient<INasaReplyReader, NasaReplyWebReader>();
        }

        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
