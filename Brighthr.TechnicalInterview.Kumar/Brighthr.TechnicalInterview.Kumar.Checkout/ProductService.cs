using Brighthr.TechnicalInterview.Kumar.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brighthr.TechnicalInterview.Kumar.Checkout
{
    public interface IProductService
    {
        void CreateProduct(Product product);
        Product ReadProduct(int productId);
        void UpdateProduct(Product updatedProduct);
        void DeleteProduct(int productId);
    }


    /// <summary>
    /// Ideally This need to be in a different project for simplicity have included in the checkout project.
    /// Deals with basic CRUD operation.Could have assumed that this already exists but given its simple to create used it to get complete picture.
    /// </summary>
    public class ProductService:IProductService
    {
        private IDataStore dataStore;

        public ProductService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public void CreateProduct(Product product)
        {
            dataStore.Products.Add(product);
        }

        public Product ReadProduct(int productId)
        {
            return dataStore.Products.FirstOrDefault(p => p.Id == productId);
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var existingProduct = dataStore.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.SKU = updatedProduct.SKU;
                existingProduct.Price = updatedProduct.Price;
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = dataStore.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                dataStore.Products.Remove(product);
            }
        }
    }


}
