using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout;

public interface IProductService
{
    void CreateProduct(Product product);
    Product ReadProduct(int productId);
    void UpdateProduct(Product updatedProduct);
    void DeleteProduct(int productId);
    Product ReadProductBySKU(string sku);
}