using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Shared;

namespace Application.Features.Categories.Queries.Get;

public sealed record GetCategoriesQuery : IQuery<List<CategoryResponse>>;

internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryResponse>>> Handle(
        GetCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAsync(cancellationToken);
        var categoryResonses = _mapper.Map<List<CategoryResponse>>(categories);

        return categoryResonses;
    }
}