namespace Persistence.Bogus;

public interface IDataSeeder
{
    Task SeedAsync();

    Task SeedIncomesAsync();

    Task GetUserFromJson(string path);

    Task GetWarehousesFromJson(string path);

    Task GetSuppliersFromJson(string path);
}