using Microsoft.Extensions.Logging;
using Ksiazka_Adresowa.Converters;
using CommunityToolkit.Maui;

namespace Ksiazka_Adresowa;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit() // Dodana linia inicjalizująca CommunityToolkit
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<LocalDbService>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<EditContactPage>();
        builder.Services.AddTransient<ContactDetailPage>();

        builder.Services.AddSingleton<CountToVisibilityConverter>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}