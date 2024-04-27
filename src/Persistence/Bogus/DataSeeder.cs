using Application.Abstractions;
using Bogus.Bson;
using Domain.Entities.Categories;
using Domain.Entities.Deliveries;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Domain.Entities.Suppliers;
using Domain.Entities.SupplyProducts;
using Domain.Entities.Users;
using Domain.Entities.WarehouseProducts;
using Domain.Entities.Warehouses;
using Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using Persistence.Bogus.Fakers;

namespace Persistence.Bogus;

public class DataSeeder : IDataSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly ISupplyProductRepository _supplyProductRepository;
    private readonly IWarehouseProductRepository _warehouseProductRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    private readonly IUnitOfWork _unitOfWork;

    public DataSeeder(
        IUserRepository userRepository,
        IProductRepository productRepository, 
        IOrderRepository orderRepository, 
        ICategoryRepository categoryRepository, 
        IDeliveryRepository deliveryRepository, 
        ISupplierRepository supplierRepository, 
        ISupplyProductRepository supplyProductRepository, 
        IWarehouseProductRepository warehouseProductRepository, 
        IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _categoryRepository = categoryRepository;
        _deliveryRepository = deliveryRepository;
        _supplierRepository = supplierRepository;
        _supplyProductRepository = supplyProductRepository;
        _warehouseProductRepository = warehouseProductRepository;
        _warehouseRepository = warehouseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var furnitureCategories = new List<Category>
        {
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Living Room")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Bedroom")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Dining Room")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Home Office")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Outdoor")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Kitchen")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Bathroom")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Kids Room")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Entryway")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Entertainment")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Storage")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Bar Furniture")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Home Décor")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Accent Furniture")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Office Furniture")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Laundry Room")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Garage")),
            Category.Create(new CategoryId(Guid.NewGuid()), Name.Create("Pet Furniture")),
        };


        ProductFaker productFaker = new(furnitureCategories);
        var products = productFaker.Generate(1000);

        SupplierFaker supplierFaker = new();
        var suppliers = supplierFaker.Generate(1000);

        SupplyProductFaker supplyProductFaker = new(suppliers, products);
        var supplyProducts = new List<SupplyProduct>();

        WarehouseFaker warehouseFaker = new();
        var warehouses = warehouseFaker.Generate(100);

        WarehouseProductFaker warehouseProductFaker = new(warehouses);
        var warehouseProducts = new List<WarehouseProduct>();

        for (int i = 0; i < 1000; i++)
        {
            var supplyProduct = supplyProductFaker.Generate();
            supplyProducts.Add(supplyProduct);

            var warehouseProduct = warehouseProductFaker.Generate();
            warehouseProduct.Update(supplyProduct.ProductId, warehouseProduct.WarehouseId, supplyProduct.Quantity);
            warehouseProducts.Add(warehouseProduct);
        }

        ExpenseFaker expenseFaker = new(supplyProducts);
        var expenses = expenseFaker.Generate(3000);

        foreach (var supplyProduct in supplyProducts)
        {
            var expensesPerSupply = expenses.Where(e => e.SupplyProductId == supplyProduct.Id).ToList();

            var amount = products
                .Where(p => p.Id == supplyProduct.ProductId)
                .Select(p => p.Price.Value)
                .SingleOrDefault() * supplyProduct.Quantity.Value * 0.9;

            foreach (var expense in expensesPerSupply)
            {
                expense.SetAmount(amount / expensesPerSupply.Count);
            }

            supplyProduct.AddExpenses(expensesPerSupply);
        }

        UserFaker userFaker = new();
        var users = new List<User>();

        for (int i = 0; i < 500; i++)
        {
            var user = userFaker.Generate();
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            users.Add(user);
        }

        OrderFaker orderFaker = new(users);
        var orders = orderFaker.Generate(50);

        OrderDetailFaker orderDetailFaker = new(orders, products);
        var orderDetails = orderDetailFaker.Generate(1000);

        foreach (var order in orders)
        {
            var newOrderDetails = orderDetails.Where(o => o.OrderId == order.Id).ToList();

            order.AddOrderDetails(newOrderDetails);
        }

        DeliveryFaker deliveryFaker = new(orders);
        var deliveries = deliveryFaker.Generate(50);

        for (int i = 0; i < deliveries.Count; i++)
        {
            deliveries[i].Update(orders[i].Id, orders[i].CreatedAtUtc, deliveries[i].Address, orders[i].CreatedAtUtc.AddDays(2), true);
            orders[i].SetDelivery(deliveries[i].Id);
        }

        await _productRepository.AddRangeAsync(products);
        await _categoryRepository.AddRangeAsync(furnitureCategories);
        await _warehouseProductRepository.AddRangeAsync(warehouseProducts);
        await _supplyProductRepository.AddRangeAsync(supplyProducts);
        await _supplierRepository.AddRangeAsync(suppliers);
        await _warehouseRepository.AddRangeAsync(warehouses);
        //await _userRepository.AddRangeAsync(users);
        await _orderRepository.AddRangeAsync(orders);
        await _deliveryRepository.AddRangeAsync(deliveries);

        //string json = JsonConvert.SerializeObject(users);
        //File.WriteAllText("C:\\testjson\\test.json", json);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task SeedIncomesAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        IncomeFaker incomeFaker = new(orders);

        foreach (var order in orders)
        {
            Random rnd = new Random();
            var incomesPerOrder = incomeFaker.Generate(rnd.Next(1, 4));

            foreach (var income in incomesPerOrder)
            {
                income.SetAmount(order.TotalAmount.Value / incomesPerOrder.Count);
            }

            order.AddIncomes(incomesPerOrder);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task GetUserFromJson(string path)
    {
        if (File.Exists(path))
        {
            string json = await File.ReadAllTextAsync(path);

            var userList = JsonConvert.DeserializeObject<List<User>>(json);

            if (userList != null)
            {
                await _userRepository.AddRangeAsync(userList);

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }

    public async Task GetWarehousesFromJson(string path)
    {
        if (File.Exists(path))
        {
            string json = await File.ReadAllTextAsync(path);

            var warehouseList = JsonConvert.DeserializeObject<List<Warehouse>>(json);

            if (warehouseList != null)
            {
                await _warehouseRepository.AddRangeAsync(warehouseList);

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }

    public async Task GetSuppliersFromJson(string path)
    {
        if (File.Exists(path))
        {
            string json = await File.ReadAllTextAsync(path);

            var supplierList = JsonConvert.DeserializeObject<List<Supplier>>(json);

            if (supplierList != null)
            {
                await _supplierRepository.AddRangeAsync(supplierList);

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}