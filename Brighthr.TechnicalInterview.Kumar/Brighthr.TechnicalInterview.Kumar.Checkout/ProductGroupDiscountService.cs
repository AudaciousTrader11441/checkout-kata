using Brighthr.TechnicalInterview.Kumar.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brighthr.TechnicalInterview.Kumar.Checkout
{
    public interface IProductGroupDiscountService
    {
        void CreateProductGroupDiscount(ProductGroupDiscount discount);
        ProductGroupDiscount ReadProductGroupDiscount(int discountId);
        void UpdateProductGroupDiscount(ProductGroupDiscount updatedDiscount);
        void DeleteProductGroupDiscount(int discountId);
    }

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
