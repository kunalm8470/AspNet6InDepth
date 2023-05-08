namespace Api.Models;

public abstract class BaseEntity<TKey> where TKey : notnull
{
    public virtual TKey Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
