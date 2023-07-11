using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brighthr.TechnicalInterview.Kumar.Checkout;

namespace Brighthr.TechnicalInterview.Kumar.DataStore.TestSuit
{

    [TestFixture]
    public class ProductGroupDiscountServiceTests
    {
        private IDataStore dataStore;
        private IProductGroupDiscountService discountService;

        [SetUp]
        public void Initialize()
        {
            dataStore = new InMemory();
            discountService = new ProductGroupDiscountService(dataStore);
        }

        [Test]
        public void CreateProductGroupDiscount_ShouldAddNewDiscountToDataStore_WithExistingProduct_WithIdPassed()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99f
            };
            dataStore.Products.Add(product);
            var discount = new ProductGroupDiscount
            {
                Id = 10, // Ignoring the passed-in ID
                ProductId = 1,
                DiscountName = "Example Discount",
                Price = 19.99f,
                ProductCount = 5
            };

            // Act
            discountService.CreateProductGroupDiscount(discount);

            // Assert
            Assert.AreEqual(1, dataStore.ProductGroupDiscounts.Count);
            var createdDiscount = dataStore.ProductGroupDiscounts[0];
            Assert.AreEqual(discount.ProductId, createdDiscount.ProductId);
            Assert.AreEqual(discount.DiscountName, createdDiscount.DiscountName);
            Assert.AreEqual(discount.Price, createdDiscount.Price);
            Assert.AreEqual(discount.ProductCount, createdDiscount.ProductCount);
            Assert.IsTrue(createdDiscount.Id != 10); // Assert that the ID is not equal to the passed-in ID
            Assert.IsTrue(createdDiscount.Id > 0); // Assert that the ID is greater than 0
        }



        [Test]
        public void CreateProductGroupDiscount_ShouldAddNewDiscountToDataStore_WithExistingProduct_WithOutID()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99f
            };
            dataStore.Products.Add(product);

            var discount = new ProductGroupDiscount
            {
                ProductId = product.Id,
                DiscountName = "Example Discount",
                Price = 19.99f,
                ProductCount = 5
            };

            // Act
            discountService.CreateProductGroupDiscount(discount);

            // Assert
            Assert.AreEqual(1, dataStore.ProductGroupDiscounts.Count);
            var createdDiscount = dataStore.ProductGroupDiscounts[0];
            Assert.AreEqual(discount.ProductId, createdDiscount.ProductId);
            Assert.AreEqual(discount.DiscountName, createdDiscount.DiscountName);
            Assert.AreEqual(discount.Price, createdDiscount.Price);
            Assert.AreEqual(discount.ProductCount, createdDiscount.ProductCount);
            Assert.IsTrue(createdDiscount.Id > 0); // Assert that the ID is greater than 0
        }

        [Test]
        public void CreateProductGroupDiscount_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var discount = new ProductGroupDiscount
            {
                ProductId = 1, // Non-existent product ID
                DiscountName = "Example Discount",
                Price = 19.99f,
                ProductCount = 5
            };

            // Act and Assert
            Assert.Throws<ArgumentException>(() => discountService.CreateProductGroupDiscount(discount));
        }

        [Test]
        public void UpdateProductGroupDiscount_ShouldUpdateExistingDiscount_WithExistingProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99f
            };
            dataStore.Products.Add(product);

            var existingDiscount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Existing Discount",
                Price = 19.99f,
                ProductCount = 5
            };
            dataStore.ProductGroupDiscounts.Add(existingDiscount);

            var updatedDiscount = new ProductGroupDiscount
            {
                Id = existingDiscount.Id,
                ProductId = product.Id,
                DiscountName = "Updated Discount",
                Price = 29.99f,
                ProductCount = 10
            };

            // Act
            discountService.UpdateProductGroupDiscount(updatedDiscount);

            // Assert
            var result = dataStore.ProductGroupDiscounts.FirstOrDefault(d => d.Id == existingDiscount.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedDiscount.DiscountName, result.DiscountName);
            Assert.AreEqual(updatedDiscount.Price, result.Price);
            Assert.AreEqual(updatedDiscount.ProductCount, result.ProductCount);
        }

        [Test]
        public void UpdateProductGroupDiscount_ShouldThrowException_WhenDiscountDoesNotExist()
        {
            // Arrange
            var updatedDiscount = new ProductGroupDiscount
            {
                Id = 1, // Non-existent discount ID
                ProductId = 1,
                DiscountName = "Updated Discount",
                Price = 29.99f,
                ProductCount = 10
            };

            // Act and Assert
            Assert.Throws<ArgumentException>(() => discountService.UpdateProductGroupDiscount(updatedDiscount));
        }

        [Test]
        public void UpdateProductGroupDiscount_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var existingDiscount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = 1, // Non-existent product ID
                DiscountName = "Existing Discount",
                Price = 19.99f,
                ProductCount = 5
            };
            dataStore.ProductGroupDiscounts.Add(existingDiscount);

            var updatedDiscount = new ProductGroupDiscount
            {
                Id = existingDiscount.Id,
                ProductId = 2, // Non-existent product ID
                DiscountName = "Updated Discount",
                Price = 29.99f,
                ProductCount = 10
            };

            // Act and Assert
            Assert.Throws<ArgumentException>(() => discountService.UpdateProductGroupDiscount(updatedDiscount));
        }

        [Test]
        public void ReadProductGroupDiscount_ShouldReturnExistingDiscountFromDataStore()
        {
            // Arrange
            var discount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = 1,
                DiscountName = "Example Discount",
                Price = 19.99f,
                ProductCount = 5
            };
            dataStore.ProductGroupDiscounts.Add(discount);

            // Act
            var result = discountService.ReadProductGroupDiscount(1);

            // Assert
            Assert.AreEqual(discount, result);
        }

        [Test]
        public void DeleteProductGroupDiscount_ShouldRemoveExistingDiscount_WithExistingProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99f
            };
            dataStore.Products.Add(product);

            var existingDiscount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Existing Discount",
                Price = 19.99f,
                ProductCount = 5
            };
            dataStore.ProductGroupDiscounts.Add(existingDiscount);

            // Act
            discountService.DeleteProductGroupDiscount(existingDiscount.Id);

            // Assert
            Assert.AreEqual(0, dataStore.ProductGroupDiscounts.Count);
        }

        [Test]
        public void DeleteProductGroupDiscount_ShouldThrowException_WhenDiscountDoesNotExist()
        {
            // Arrange
            var discountId = 1; // Non-existent discount ID

            // Act and Assert
            Assert.Throws<ArgumentException>(() => discountService.DeleteProductGroupDiscount(discountId));
        }

        

    }

}
