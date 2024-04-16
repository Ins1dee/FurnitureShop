using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(Guid ProductToDeleteId) : ICommand;

public sealed class DeleteProductHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? productToDelete = await _productRepository
            .GetByIdAsync(new ProductId(request.ProductToDeleteId), cancellationToken);

        if (productToDelete is null)
        {
            return Result.NotFound(DomainErrors.Product.NotFound());
        }
        
        _productRepository.Delete(productToDelete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}