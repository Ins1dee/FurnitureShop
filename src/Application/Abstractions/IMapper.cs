namespace Application.Abstractions;

public interface IMapper
{
    TTarget Map<TTarget>(object source);
}