using System.Text.RegularExpressions;
using CommunityToolkit.Maui.Alerts;

namespace Ksiazka_Adresowa;

[QueryProperty(nameof(CustomerId), "id")]
public partial class EditContactPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private Customer _customer;

    public string CustomerId { get; set; }

    public EditContactPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        _customer = new Customer();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (string.IsNullOrWhiteSpace(CustomerId) || !int.TryParse(CustomerId, out int id))
        {
            Title = "Dodaj kontakt"; 
            return;
        }

        _customer = await _dbService.GetById(id);
        if (_customer != null)
        {
            Title = "Edytuj kontakt";
            LoadCustomerData();
        }
        else
        {
            await Toast.Make("Nie znaleziono kontaktu do edytowania").Show();
            await Shell.Current.GoToAsync("..");
        }
    }

    private void LoadCustomerData()
    {
        nameEntryField.Text = _customer.CustomerName;
        emailEntryField.Text = _customer.Email;
        mobileEntryField.Text = _customer.Mobile;
        addressEntryField.Text = _customer.Address;
        cityEntryField.Text = _customer.City;
        postalCodeEntryField.Text = _customer.PostalCode;
        notesEntryField.Text = _customer.Notes;
    }

    private bool IsValidEmail(string email) => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    private bool IsValidPostalCode(string postalCode) => Regex.IsMatch(postalCode, @"^\d{2}-\d{3}$");

    private bool IsValidMobile(string mobile) => Regex.IsMatch(mobile, @"^\d{3}-\d{3}-\d{3}$");

    private void OnFieldChanged(object sender, TextChangedEventArgs e)
    {
        nameErrorLabel.IsVisible = string.IsNullOrWhiteSpace(nameEntryField.Text);
        nameErrorLabel.Text = "Imię i nazwisko nie może być puste";

        emailErrorLabel.IsVisible = !string.IsNullOrWhiteSpace(emailEntryField.Text) && !IsValidEmail(emailEntryField.Text);
        emailErrorLabel.Text = "Nieprawidłowy email (np. test@domena.pl)";

        mobileErrorLabel.IsVisible = string.IsNullOrWhiteSpace(mobileEntryField.Text) || !IsValidMobile(mobileEntryField.Text);
        mobileErrorLabel.Text = "Telefon musi być w formacie 123-456-789";

        bool addressFilled = !string.IsNullOrWhiteSpace(addressEntryField.Text);
        addressErrorLabel.IsVisible = false;
        cityErrorLabel.IsVisible = false;
        postalCodeErrorLabel.IsVisible = false;

        if (addressFilled)
        {
            cityErrorLabel.IsVisible = string.IsNullOrWhiteSpace(cityEntryField.Text);
            cityErrorLabel.Text = "Miasto jest wymagane";

            postalCodeErrorLabel.IsVisible = string.IsNullOrWhiteSpace(postalCodeEntryField.Text) || !IsValidPostalCode(postalCodeEntryField.Text);
            postalCodeErrorLabel.Text = "Kod pocztowy w formacie xx-xxx";
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        OnFieldChanged(null, null);

        if (nameErrorLabel.IsVisible || emailErrorLabel.IsVisible || mobileErrorLabel.IsVisible || addressErrorLabel.IsVisible || cityErrorLabel.IsVisible || postalCodeErrorLabel.IsVisible)
        {
            await Toast.Make("Popraw błędy w formularzu").Show();
            return;
        }

        _customer.CustomerName = nameEntryField.Text;
        _customer.Email = emailEntryField.Text;
        _customer.Mobile = mobileEntryField.Text;
        _customer.Address = addressEntryField.Text;
        _customer.City = cityEntryField.Text;
        _customer.PostalCode = postalCodeEntryField.Text;
        _customer.Notes = notesEntryField.Text;

        if (_customer.Id == 0)
        {
            await _dbService.Create(_customer);
            await Toast.Make("Kontakt został dodany").Show();
        }
        else
        {
            await _dbService.Update(_customer);
            await Toast.Make("Kontakt został zaktualizowany").Show();
        }

        await Shell.Current.GoToAsync("..");
    }
}