namespace IWantApp.Domain.Products;

public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasStock { get; set; }
    public bool Active { get; set; } = true;

    public virtual Category Category { get; set; }
    public Guid CategoryId { get; set; }

    public Product(string name, string description, bool hasStock, Guid categoryId, string createdBy, string editedBy)
    {
        Name = name;
        Description = description;
        HasStock = hasStock;
        CategoryId = categoryId;
        Active = true;
        CreatedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedBy = editedBy;
        EditedOn = DateTime.Now;
    }
}
