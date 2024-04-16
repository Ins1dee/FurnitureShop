using Application.Abstractions;
using Application.Abstractions.Idempotency;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain.Entities.Deliveries;
using Domain.Entities.Orders;
using Domain.Entities.Orders.ValueObjects;
using Domain.Entities.Products;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;
using FluentValidation;

namespace Application.Features.Orders.Commands.Create;

public sealed record CreateOrderCommand(
    Guid RequestId,
    Dictionary<Guid, int> Products,
    string CustomerName,
    string CustomerPhone,
    double PaymentAmount,
    Guid? DeliveryId = default,
    string? DeliveryAddress = default) : IdempotentCommand(RequestId);

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotNull();

        RuleFor(x => x.Products)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.CustomerName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(CustomerDetails.NameMaxLength);
        
        RuleFor(x => x.CustomerPhone)
            .NotNull()
            .NotEmpty()
            .MaximumLength(CustomerDetails.PhoneMaxLength);
    }
}

public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly ISessionService _sessionService;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(
        ISessionService sessionService,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository)
    {
        _sessionService = sessionService;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        User? user = await _sessionService.GetLoggedInUserAsync(cancellationToken);

        if (user is null)
        {
            return Result.Unauthorized(DomainErrors.User.Unauthorized());
        }

        var productIds = request.Products.Keys
            .Select(key => new ProductId(key))
            .ToList();
        
        var products = await _productRepository.GetRangeById(productIds, cancellationToken);
        var values = request.Products.Values.ToList();

        if (products is null)
        {
            return Result.BadRequest(DomainErrors.Product.RangeNotFound());
        }
        
        var productsWithQuantity = new Dictionary<Product, int>();
        productsWithQuantity.AddRange(products, values);

        var deliveryId = request.DeliveryId is not null 
            ? new DeliveryId((Guid)request.DeliveryId)
            : null;

        Order order = Order.Create(
            new OrderId(Guid.NewGuid()),
            user, productsWithQuantity,
            CustomerDetails.Create(
                request.CustomerName,
                request.CustomerPhone),
            request.PaymentAmount,
            deliveryId,
            request.DeliveryAddress);

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}