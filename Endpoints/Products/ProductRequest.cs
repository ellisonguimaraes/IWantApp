namespace IWantApp.Endpoints.Products;

public record ProductRequest(string Name, string Description, bool HasStock, Guid CategoryId);