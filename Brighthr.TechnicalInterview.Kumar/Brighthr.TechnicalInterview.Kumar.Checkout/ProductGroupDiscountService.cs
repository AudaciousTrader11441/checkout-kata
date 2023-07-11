using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout
{
    public class ProductGroupDiscountService : IProductGroupDiscountService
    {
        private IDataStore dataStore;

        public ProductGroupDiscountService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public void CreateProductGroupDiscount(ProductGroupDiscount discount)
        {
            if (dataStore.Products.Any(p => p.Id == discount.ProductId))
            {
                discount.Id = GenerateNewId();
                dataStore.ProductGroupDiscounts.Add(discount);
            }
            else
            {
                throw new ArgumentException("Product does not exist.");
            }
        }


        public ProductGroupDiscount ReadProductGroupDiscount(int discountId)
        {
            return dataStore.ProductGroupDiscounts.FirstOrDefault(d => d.Id == discountId);
        }

        public void UpdateProductGroupDiscount(ProductGroupDiscount updatedDiscount)
        {
            if (dataStore.Products.Any(p => p.Id == updatedDiscount.ProductId))
            {
                var existingDiscount = dataStore.ProductGroupDiscounts.FirstOrDefault(d => d.Id == updatedDiscount.Id);
                if (existingDiscount != null)
                {
                    existingDiscount.ProductId = updatedDiscount.ProductId;
                    existingDiscount.DiscountName = updatedDiscount.DiscountName;
                    existingDiscount.Price = updatedDiscount.Price;
                    existingDiscount.ProductCount = updatedDiscount.ProductCount;
                }
                else
                {
                    throw new ArgumentException("Discount does not exist.");
                }
            }
            else
            {
                throw new ArgumentException("Product does not exist.");
            }
        }


        public void DeleteProductGroupDiscount(int discountId)
        {
            var discount = dataStore.ProductGroupDiscounts.FirstOrDefault(d => d.Id == discountId);
            if (discount != null)
            {

                dataStore.ProductGroupDiscounts.Remove(discount);

            }
            else
            {
                throw new ArgumentException("Product discount associated with the discount does not exist.");
            }
        }

        public List<IDiscount> GetApplicableDiscounts(int productId, int productCount)
        {
            var discounts = dataStore.ProductGroupDiscounts
                .Where(d => d.ProductId == productId && d.ProductCount <= productCount)
                .ToList();

            return discounts.Cast<IDiscount>().ToList();
        }



        private int GenerateNewId()
        {
            if (dataStore.ProductGroupDiscounts.Count > 0)
            {
                // Generate a new ID by incrementing the maximum existing ID by 1
                int maxId = dataStore.ProductGroupDiscounts.Max(d => d.Id);
                return maxId + 1;
            }
            else
            {
                // If no discounts exist, start with ID 1
                return 1;
            }
        }

    }

}
