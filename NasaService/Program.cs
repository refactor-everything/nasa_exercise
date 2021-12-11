using NasaService;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        var config = app.AddUserSecrets<Program>()
        .Build();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IImageGetter, NasaImageGetter>();
    })
    .Build();

await host.RunAsync();
