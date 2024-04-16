using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Errors;
using Domain.Shared;

namespace Application.Features.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;

public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetByIdAsync(
            new CategoryId(request.Id), 
            cancellationToken);

        if (category is null)
        {
            return Result.NotFound(DomainErrors.Category.NotFound());
        }

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}