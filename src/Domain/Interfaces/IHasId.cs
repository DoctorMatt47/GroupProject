namespace GroupProject.Domain.Interfaces;

public interface IHasId<out T>
{
    public T Id { get; }
}
