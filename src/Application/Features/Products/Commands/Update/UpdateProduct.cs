using Application.Abstractions;
using Application.Abstractions.Messaging;
using FluentValidation;
using Application.Abstractions.Idempotency;
using Domain.Entities.Categories;
using Domain.Entities.Products;
using Domain.Entities.Products.ValueObjects;
using Domain.Errors;
using Domain.Shared;
using Domain.Shared.ValueObjects;
using Serilog;

namespace Application.Features.Products.Commands.Update;

public sealed record UpdateProductCommand(
    Guid ProductToUpdateId,
    string Name,
    string Description,
    string BrandCompany,
    string BrandCountry,
    double Price,
    double Width,
    double Height,
    double Length,
    List<string> Categories) : ICommand;
    
public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
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
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Width)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Height)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Length)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Categories)
            .NotNull()
            .NotEmpty();
    }
}

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public UpdateProductCommandHandler(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork,
        ILogger logger)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? productToUpdate = await _productRepository.GetByIdAsync(
            new ProductId(request.ProductToUpdateId), 
            cancellationToken);

        var categories = await _categoryRepository.GetRangeByName(request.Categories, cancellationToken);
        
        if (productToUpdate is null)
        {
            _logger.Error($"Error occured trying to update product with id {request.ProductToUpdateId}. " +
                          $"Error message: {DomainErrors.Product.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.Product.NotFound());
        }

        if (categories is null)
        {
            _logger.Error($"Error occured trying to update product with id {request.ProductToUpdateId}. " +
                          $"Error message: {DomainErrors.Category.RangeNotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.Category.RangeNotFound());
        }

        productToUpdate.Update(
            Name.Create(request.Name),
            Description.Create(request.Description),
            Brand.Create(request.BrandCompany, request.BrandCountry),
            Price.Create(request.Price),
            Dimensions.Create(request.Width, request.Height, request.Length),
            categories);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Product with id {request.ProductToUpdateId} was successfully updated, status code: 200");

        return Result.Success();
    }
}