using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Errors;
using Domain.Shared;
using Domain.Shared.ValueObjects;
using FluentValidation;

namespace Application.Features.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(Guid CategoryToUpdateId, string Name) : ICommand;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Name.MaxLength)
            .MustAsync(async (name, _) => await categoryRepository.IsNameUniqueAsync(name));
    }
}

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetByIdAsync(
            new CategoryId(request.CategoryToUpdateId), 
            cancellationToken);

        if (category is null)
        {
            return Result.NotFound(DomainErrors.Category.NotFound());
        }
        
        category.Update(Name.Create(request.Name));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}