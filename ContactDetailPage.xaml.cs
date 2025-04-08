namespace Ksiazka_Adresowa;

public partial class ContactDetailPage : ContentPage
{
    private readonly LocalDbService _dbService;
    private Customer _customer;

    public ContactDetailPage(LocalDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
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
                
                detailName.Text = _customer.CustomerName;
                detailMobile.Text = $"Tel: {_customer.Mobile}";
                detailEmail.Text = $"Email: {_customer.Email}";
                detailAddress.Text = $"Adres: {_customer.Address}";
                detailCityPostal.Text = $"{_customer.PostalCode} {_customer.City}";
                detailNotes.Text = _customer.Notes;
            }
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?id={_customer.Id}");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usunąć ten kontakt?", "Tak", "Nie");
        if (confirm)
        {
            await _dbService.Delete(_customer);
            await DisplayAlert("Sukces", "Kontakt został usunięty", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}