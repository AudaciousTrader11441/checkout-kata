namespace Brighthr.TechnicalInterview.Kumar.DataStore;

public interface IDataStore
{
    List<Product> Products { get; set; }
    List<ProductGroupDiscount> ProductGroupDiscounts { get; set; }
    List<Cart> Carts { get; set; }
}