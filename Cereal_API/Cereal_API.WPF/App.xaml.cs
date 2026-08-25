using Cereal_API.WPF.Services;
using Cereal_API.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;

namespace Cereal_API.WPF;

public partial class App : Application
{
    public const string cerealApiBaseAddress = "https://localhost:7196/";

    public static IServiceProvider? Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri(cerealApiBaseAddress)
        });

        services.AddSingleton<CerealHttpService>();

        services.AddTransient<CerealDataTableViewModel>();
        services.AddTransient<LoginViewModel>();

        services.AddSingleton<MainViewModel>();

        services.AddSingleton<MainWindow>();

        Services = services.BuildServiceProvider();

        var window = Services.GetRequiredService<MainWindow>();
        window.Show();

        base.OnStartup(e);
    }
}
