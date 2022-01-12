using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{

    public string Name { get; set; }
    public bool Active { get; set; }

    public virtual Product Product { get; set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
    }
}
