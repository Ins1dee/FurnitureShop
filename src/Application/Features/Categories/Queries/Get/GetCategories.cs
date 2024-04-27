using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Categories;
using Domain.Shared;
using Serilog;

namespace Application.Features.Categories.Queries.Get;

public sealed record GetCategoriesQuery : IQuery<List<CategoryResponse>>;

internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCategoriesQueryHandler(
        ICategoryRepository categoryRepository, 
        IMapper mapper, 
        ILogger logger)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<CategoryResponse>>> Handle(
        GetCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAsync(cancellationToken);
        var categoryResonses = _mapper.Map<List<CategoryResponse>>(categories);

        _logger.Information($"Returning list of categories in response to GET method" +
                            $" for all categories, status code: 200");

        return categoryResonses;
    }
}