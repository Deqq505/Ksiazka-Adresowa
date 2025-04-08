namespace Ksiazka_Adresowa;

public partial class App : Application
{
    public App(MainPage mainPage)
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}