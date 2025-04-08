using SQLite;

namespace Ksiazka_Adresowa;

public class LocalDbService
{
    private const string DB_NAME = "Ksiazka_Adresowa.db3";
    private readonly SQLiteAsyncConnection _connection;

    public LocalDbService()
    {
        _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
        InitializeDatabase();
    }

    private async Task InitializeDatabase()
    {
        await _connection.CreateTableAsync<Customer>();
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await _connection.Table<Customer>().OrderBy(c => c.CustomerName).ToListAsync();
    }

    public async Task<Customer> GetById(int id)
    {
        return await _connection.Table<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task Create(Customer customer)
    {
        await _connection.InsertAsync(customer);
    }

    public async Task Update(Customer customer)
    {
        await _connection.UpdateAsync(customer);
    }

    public async Task Delete(Customer customer)
    {
        await _connection.DeleteAsync(customer);
    }
}