namespace Ksiazka_Adresowa;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EditContactPage), typeof(EditContactPage));
        Routing.RegisterRoute(nameof(ContactDetailPage), typeof(ContactDetailPage));
    }
}