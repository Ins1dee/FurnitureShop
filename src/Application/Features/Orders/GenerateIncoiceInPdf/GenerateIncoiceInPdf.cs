using Application.Abstractions;
using Application.Abstractions.Idempotency;
using Application.Abstractions.Messaging;
using Domain.Entities.Orders;
using Domain.Errors;
using Domain.Shared;
using Razor.Templating.Core;
using Serilog;

namespace Application.Features.Orders.GenerateIncoiceInPdf;

public sealed record GenerateIncoiceInPdfCommand(
    Guid RequestId, 
    Guid OrderId) : IdempotentCommand<byte[]>(RequestId);

public sealed class GenerateIncoiceInPdfCommandHandler : ICommandHandler<GenerateIncoiceInPdfCommand, byte[]>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;
    private readonly IReportService _reportService;

    public GenerateIncoiceInPdfCommandHandler(
        IOrderRepository orderRepository, 
        ILogger logger, 
        IReportService reportService)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _reportService = reportService;
    }

    public async Task<Result<byte[]>> Handle(GenerateIncoiceInPdfCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);

        if (order is null)
        {
            _logger.Error($"Error occured trying to generate invoice for order with id {request.OrderId}" +
                          $"Error message: {DomainErrors.Order.NotFound()}. " +
                          $"Status code: 404");

            return Result.NotFound<byte[]>(DomainErrors.Order.NotFound());
        }

        var html = await _reportService.GenerateHtmlFromRazorPage("OrderReport", order);

        var render = new ChromePdfRenderer();

        using var pdfDocument = render.RenderHtmlAsPdf(html);

        return pdfDocument.BinaryData;
    }
}