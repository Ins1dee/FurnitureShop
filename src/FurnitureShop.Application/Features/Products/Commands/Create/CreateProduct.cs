using FluentValidation;
using FurnitureShop.Application.Abstractions;
using FurnitureShop.Application.Abstractions.Idempotency;
using FurnitureShop.Application.Abstractions.Messaging;
using FurnitureShop.Domain.Entities.Categories;
using FurnitureShop.Domain.Entities.Products;
using FurnitureShop.Domain.Entities.Products.ValueObjects;
using FurnitureShop.Domain.Errors;
using FurnitureShop.Domain.Shared;
using FurnitureShop.Domain.Shared.ValueObjects;

namespace FurnitureShop.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    Guid RequestId,
    string Name,
    string Description,
    string BrandCompany,
    string BrandCountry,
    double Price,
    double Width,
    double Height,
    double Length,
    List<string> Categories) : IdempotentCommand(RequestId);

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Name.MaxLength);
        
        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Description.MaxLength);

        RuleFor(x => x.BrandCompany)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Brand.NameMaxLength);
        
        RuleFor(x => x.BrandCountry)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Brand.NameMaxLength);

        RuleFor(x => x.Price)
            .NotNull();

        RuleFor(x => x.Width)
            .NotNull();

        RuleFor(x => x.Height)
            .NotNull();

        RuleFor(x => x.Length)
            .NotNull();

        RuleFor(x => x.Categories)
            .NotNull()
            .NotEmpty();
    }
}

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetRangeByName(request.Categories, cancellationToken);

        if (categories is null)
        {
            return Result.Failure(DomainErrors.Category.RangeNotFound());
        }

        Product product = Product.Create(new ProductId(Guid.NewGuid()),
            Name.Create(request.Name),
            Description.Create(request.Description),
            Brand.Create(request.BrandCompany, request.BrandCountry),
            Price.Create(request.Price),
            Dimentions.Create(request.Width, request.Height, request.Length),
            categories);

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}