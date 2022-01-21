namespace IWantApp.Endpoints.Products;

public record ProductResponse(Guid Id, string Name, string Description, bool HasStock);
