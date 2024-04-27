using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Shared;
using Domain.Shared.ValueObjects;
using FluentValidation;
using Serilog;

namespace Application.Features.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name) : ICommand;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Name.MaxLength)
            .MustAsync(async (name, _) => await categoryRepository.IsNameUniqueAsync(name));
    }
}

public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork, 
        ILogger logger)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = Category.Create(
            new CategoryId(Guid.NewGuid()), 
            Name.Create(request.Name));

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"New category with id {category.Id.Value} was successfully created, status code: 200");

        return Result.Success();
    }
}