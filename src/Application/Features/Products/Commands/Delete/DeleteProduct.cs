using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(Guid ProductToDeleteId) : ICommand;

public sealed class DeleteProductHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public DeleteProductHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ILogger logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? productToDelete = await _productRepository
            .GetByIdAsync(new ProductId(request.ProductToDeleteId), cancellationToken);

        if (productToDelete is null)
        {
            _logger.Error($"Error occured trying to delete product with id {request.ProductToDeleteId}. " +
                          $"Error message: {DomainErrors.Product.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.Product.NotFound());
        }
        
        _productRepository.Delete(productToDelete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Product with id {request.ProductToDeleteId} was successfully deleted, status code: 200");

        return Result.Success();
    }
}