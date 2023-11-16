namespace FurnitureShop.Domain.Shared;

public interface IResult
{
     static abstract IResult IdempotencyResult();
}