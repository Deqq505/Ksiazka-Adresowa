using Ksiazka_Adresowa;
using CommunityToolkit.Maui.Alerts;

namespace Ksiazka_Adresowa;

[QueryProperty(nameof(CustomerId), "id")]
public partial class ContactDetailPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private Customer _customer;

    public string CustomerId { get; set; }

    public ContactDetailPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            if (string.IsNullOrWhiteSpace(CustomerId) || !int.TryParse(CustomerId, out int id))
            {
                await Toast.Make("Brak identyfikatora kontaktu").Show();
                await Shell.Current.GoToAsync("..");
                return;
            }

            _customer = await _dbService.GetById(id);

            if (_customer == null)
            {
                await Toast.Make("Nie znaleziono kontaktu").Show();
                await Shell.Current.GoToAsync("..");
                return;
            }

            detailName.Text = _customer.CustomerName;
            detailMobile.Text = $"Tel: {_customer.Mobile}";
            detailEmail.Text = $"Email: {_customer.Email}";
            detailAddress.Text = $"Adres: {_customer.Address}";
            detailCityPostal.Text = $"{_customer.PostalCode} {_customer.City}";
            detailNotes.Text = _customer.Notes;
        }
        catch (Exception ex)
        {
            await Toast.Make($"Coś poszło nie tak: {ex.Message}").Show();
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (_customer == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?id={_customer.Id}");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_customer == null)
            return;

        bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć ten kontakt?", "Tak", "Nie");
        if (confirm)
        {
            await _dbService.Delete(_customer);
            await Toast.Make("Kontakt został usunięty").Show();
            await Shell.Current.GoToAsync("..");
        }
    }
}