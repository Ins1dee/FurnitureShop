using FurnitureShop.Application.Abstractions;
using FurnitureShop.Application.Abstractions.Caching;
using FurnitureShop.Application.Abstractions.Messaging;
using FurnitureShop.Domain.Entities.Products;
using FurnitureShop.Domain.Shared;

namespace FurnitureShop.Application.Features.Products.Queries.Get;

public sealed record GetProductsQuery() : IQuery<List<ProductResponse>>;

public sealed class GetProductQueryHandler : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ICashService _cashService;

    public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper, ICashService cashService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _cashService = cashService;
    }

    public async Task<Result<List<ProductResponse>>> Handle(
        GetProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var productResponses = await _cashService.GetAsync<List<ProductResponse>>("products", cancellationToken);

        if (productResponses is not null)
        {
            return productResponses;
        }
        
        var products = await _productRepository.GetAsync(cancellationToken);
        productResponses = _mapper.Map<List<ProductResponse>>(products);

        await _cashService.SetAsync("products", productResponses, cancellationToken);

        return productResponses;
    }
}