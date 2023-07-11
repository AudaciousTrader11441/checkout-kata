using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout;

public interface IProductGroupDiscountService
{
    void CreateProductGroupDiscount(ProductGroupDiscount discount);
    ProductGroupDiscount ReadProductGroupDiscount(int discountId);
    void UpdateProductGroupDiscount(ProductGroupDiscount updatedDiscount);
    void DeleteProductGroupDiscount(int discountId);
    List<IDiscount> GetApplicableDiscounts(int productId, int productCount);
}