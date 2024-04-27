using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Errors;
using Domain.Shared;
using Serilog;

namespace Application.Features.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;

public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork, 
        ILogger logger)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetByIdAsync(
            new CategoryId(request.Id), 
            cancellationToken);

        if (category is null)
        {
            _logger.Error($"An error occured tryng to delete category with id {request.Id}. " +
                          $"Error message: {DomainErrors.Category.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound(DomainErrors.Category.NotFound());
        }

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Category with id {category.Id.Value} was successfully deleted, status code: 200");

        return Result.Success();
    }
}