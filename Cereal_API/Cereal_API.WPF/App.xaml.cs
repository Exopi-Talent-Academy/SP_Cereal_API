using Cereal_API.WPF.Services;
using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Cereal_API.WPF;

public partial class App : Application
{
    public const string cerealApiBaseAddress = "http://localhost:5068/";

    public static IServiceProvider? Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri(cerealApiBaseAddress)
        });

        services.AddSingleton<CerealApiService>();
        services.AddSingleton<MainWindow>();

        Services = services.BuildServiceProvider();

        var window = Services.GetRequiredService<MainWindow>();
        window.Show();

        base.OnStartup(e);
    }
}
