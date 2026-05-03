using Microsoft.Extensions.DependencyInjection;
using Project.Views;

namespace Project;

public partial class App : Application
{
    public App(IServiceProvider services)
    {
        InitializeComponent(); // merges Colors.xaml + Styles.xaml into app resources first
        MainPage = services.GetRequiredService<LoginPage>(); // page created after resources are ready
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(MainPage!);
    }
}
