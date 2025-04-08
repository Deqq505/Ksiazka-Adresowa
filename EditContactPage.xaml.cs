namespace Ksiazka_Adresowa;

public partial class EditContactPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private Customer _customer;

    public EditContactPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        _customer = new Customer();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Shell.Current.CurrentState.Location.OriginalString.Contains("id="))
        {
            var idString = Shell.Current.CurrentState.Location.OriginalString.Split('=')[1];
            if (int.TryParse(idString, out int id))
            {
                _customer = await _dbService.GetById(id);
                Title = "Edytuj kontakt";
            }
        }
        else
        {
            Title = "Dodaj kontakt";
        }

        LoadCustomerData();
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

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameEntryField.Text) || string.IsNullOrWhiteSpace(mobileEntryField.Text))
        {
            await DisplayAlert("Błąd", "Imię i nazwisko oraz telefon są wymagane!", "OK");
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
            await DisplayAlert("Sukces", "Kontakt został dodany", "OK");
        }
        else
        {
            await _dbService.Update(_customer);
            await DisplayAlert("Sukces", "Kontakt został zaktualizowany", "OK");
        }

        await Shell.Current.GoToAsync("..");
    }
}