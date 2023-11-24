using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities.Products;
using Domain.Shared;

namespace Application.Features.Products.Queries.Get;

public sealed record GetProductsQuery() : IQuery<List<ProductResponse>>;

public sealed class GetProductQueryHandler : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductResponse>>> Handle(
        GetProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAsync(cancellationToken);
        var productResponses = _mapper.Map<List<ProductResponse>>(products);

        return productResponses;
    }
}