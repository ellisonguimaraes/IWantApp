using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{

    public string Name { get; private set; }
    public bool Active { get; private set; }

    public virtual Product Product { get; set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        Validate();
    }

    public void EditInfo(string name, bool active, string editedBy)
    {
        Name = name;
        Active = active;
        EditedBy = editedBy;
        EditedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name", "Nome é obrigatório")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "CreatedBy é obrigatório")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "EditedBy é obrigatório");
        AddNotifications(contract);
    }
}
