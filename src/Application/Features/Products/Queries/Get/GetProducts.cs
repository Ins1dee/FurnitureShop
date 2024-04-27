using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Shared;
using Serilog;

namespace Application.Features.Products.Queries.Get;

public sealed record GetProductsQuery() : IQuery<List<ProductResponse>>;

public sealed class GetProductQueryHandler : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<ProductResponse>>> Handle(
        GetProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAsync(cancellationToken);
        var productResponses = _mapper.Map<List<ProductResponse>>(products);

        _logger.Information($"Returning list of products in response to GET method" +
                            $" for all products, status code: 200");

        return productResponses;
    }
}