namespace IWantApp.Domain.Products;

public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasStock { get; set; }
    public bool Active { get; set; } = true;

    public virtual Category Category { get; set; }
    public Guid CategoryId { get; set; }
}
