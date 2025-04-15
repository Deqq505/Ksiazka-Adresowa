using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;

namespace Ksiazka_Adresowa;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    
    public ICommand ItemTappedCommand { get; }
    public ICommand AddNewContactCommand { get; }

    public MainPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        
        ItemTappedCommand = new Command<Customer>(async (customer) => 
            await Shell.Current.GoToAsync($"{nameof(ContactDetailPage)}?id={customer.Id}"));
        
        AddNewContactCommand = new Command(async () => 
            await Shell.Current.GoToAsync(nameof(EditContactPage)));
        
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            ListView.ItemsSource = await _dbService.GetCustomers();
        }
        catch (Exception ex)
        {
            await Toast.Make($"Nie można załadować kontaktów: {ex.Message}").Show();
        }
    }
}