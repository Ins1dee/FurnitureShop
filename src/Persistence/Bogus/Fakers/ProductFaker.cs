using Bogus;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Bogus.Fakers
{
    public sealed class ProductFaker : Faker<Product>
    {
        public ProductFaker(List<Category> categories)
        {
            CustomInstantiator(f =>
            {
                var randomCategories = f.PickRandom(categories, f.Random.Number(1, 3)).ToList();

                var product = Product.Create(
                    new ProductId(f.Random.Guid()),
                    Name.Create("Test"),
                    Description.Create("Test"),
                    Brand.Create("Test", "Test"),
                    Price.Create(1),
                    Dimensions.Create(1, 1, 1),
                    randomCategories);

                return product;
            });

            RuleFor(p => p.Id, f => new ProductId(f.Random.Guid()));
            RuleFor(p => p.Name, f =>
            {
                var adjective = f.Commerce.ProductAdjective();
                var category = f.PickRandom(FakerConstants.FurnitureProductNames);
                var name = f.Person.FirstName;
                var modelChar = (char)f.Random.Number('a', 'z');
                var modelNumber = f.Random.Number(1, 9);
                return Name.Create($"{adjective} {category} <<{name}>> {modelChar}{modelNumber}");
            });

            RuleFor(p => p.Brand, f => Brand.Create(f.Company.CompanyName(), f.Address.Country()));
            RuleFor(p => p.Price, f => Price.Create(Math.Round(f.Random.Double(10, 1000) / 10) * 10));
            RuleFor(p => p.Description,
                (f, p) => Description
                    .Create(
                        $"Description for {p.Name.Value}: This is a fantastic {p.Name.Value} " +
                        $"that will enhance your living space."));

            RuleFor(p => p.Dimensions,
                f => Dimensions.Create(f.Random.Double(1, 100), f.Random.Double(1, 100), f.Random.Double(1, 100)));
        }
    }
}
