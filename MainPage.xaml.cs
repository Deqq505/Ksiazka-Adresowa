namespace Ksiazka_Adresowa;

public partial class MainPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private int _editCustomerId;
    private Customer _selectedCustomer;

    public MainPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        LoadCustomers();
    }

    private async void LoadCustomers()
    {
        var customers = await _dbService.GetCustomers();
        ListView.ItemsSource = customers;
        
        if (_selectedCustomer != null)
        {
            _selectedCustomer = customers.FirstOrDefault(c => c.Id == _selectedCustomer.Id);
            if (_selectedCustomer != null)
            {
                ShowCustomerDetails(_selectedCustomer);
            }
            else
            {
                detailFrame.IsVisible = false;
            }
        }
    }

    private async void saveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameEntryField.Text) || string.IsNullOrWhiteSpace(mobileEntryField.Text))
        {
            await DisplayAlert("Błąd", "Imię i nazwisko oraz telefon są wymagane!", "OK");
            return;
        }

        var customer = new Customer
        {
            Id = _editCustomerId,
            CustomerName = nameEntryField.Text,
            Email = emailEntryField.Text,
            Mobile = mobileEntryField.Text,
            Address = addressEntryField.Text,
            City = cityEntryField.Text,
            PostalCode = postalCodeEntryField.Text,
            Notes = notesEntryField.Text
        };

        if (_editCustomerId == 0)
        {
            await _dbService.Create(customer);
            await DisplayAlert("Sukces", "Kontakt został dodany", "OK");
        }
        else
        {
            await _dbService.Update(customer);
            await DisplayAlert("Sukces", "Kontakt został zaktualizowany", "OK");
            _editCustomerId = 0;
        }

        ClearForm();
        LoadCustomers();
        detailFrame.IsVisible = false;
        _selectedCustomer = null;
    }

    private void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Customer customer)
        {
            if (_selectedCustomer != null && _selectedCustomer.Id == customer.Id)
            {
                
                detailFrame.IsVisible = !detailFrame.IsVisible;
            }
            else
            {
                
                ShowCustomerDetails(customer);
                detailFrame.IsVisible = true;
            }
            _selectedCustomer = customer;
        }
    }

    private void ShowCustomerDetails(Customer customer)
    {
        detailName.Text = customer.CustomerName;
        detailMobile.Text = $"Tel: {customer.Mobile}";
        detailEmail.Text = $"Email: {customer.Email}";
        detailAddress.Text = $"Adres: {customer.Address}";
        detailCityPostal.Text = $"{customer.PostalCode} {customer.City}";
        detailNotes.Text = customer.Notes;
    }

    private void EditDetailButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedCustomer != null)
        {
            _editCustomerId = _selectedCustomer.Id;
            nameEntryField.Text = _selectedCustomer.CustomerName;
            emailEntryField.Text = _selectedCustomer.Email;
            mobileEntryField.Text = _selectedCustomer.Mobile;
            addressEntryField.Text = _selectedCustomer.Address;
            cityEntryField.Text = _selectedCustomer.City;
            postalCodeEntryField.Text = _selectedCustomer.PostalCode;
            notesEntryField.Text = _selectedCustomer.Notes;

            detailFrame.IsVisible = false;
        }
    }

    private async void DeleteDetailButton_Clicked(object sender, EventArgs e)
    {
        if (_selectedCustomer != null)
        {
            bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć ten kontakt?", "Tak", "Nie");
            if (confirm)
            {
                await _dbService.Delete(_selectedCustomer);
                await DisplayAlert("Sukces", "Kontakt został usunięty", "OK");
                _selectedCustomer = null;
                detailFrame.IsVisible = false;
                LoadCustomers();
            }
        }
    }

    private void ClearForm()
    {
        _editCustomerId = 0;
        nameEntryField.Text = string.Empty;
        emailEntryField.Text = string.Empty;
        mobileEntryField.Text = string.Empty;
        addressEntryField.Text = string.Empty;
        cityEntryField.Text = string.Empty;
        postalCodeEntryField.Text = string.Empty;
        notesEntryField.Text = string.Empty;
    }
}