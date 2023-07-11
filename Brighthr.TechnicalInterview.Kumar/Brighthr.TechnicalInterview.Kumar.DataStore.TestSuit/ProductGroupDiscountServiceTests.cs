using Brighthr.TechnicalInterview.Kumar.Checkout;

namespace Brighthr.TechnicalInterview.Kumar.DataStore.TestSuit
{

    [TestFixture]
    public class ProductGroupDiscountServiceTests
    {
        private IDataStore _dataStore;
        private IProductGroupDiscountService _discountService;

        [SetUp]
        public void Initialize()
        {
            _dataStore = new InMemory();
            _discountService = new ProductGroupDiscountService(_dataStore);
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
                Price = 9.99m
            };
            _dataStore.Products.Add(product);
            var discount = new ProductGroupDiscount
            {
                Id = 10, // Ignoring the passed-in ID
                ProductId = 1,
                DiscountName = "Example Discount",
                Price = 19.99m,
                ProductCount = 5
            };

            // Act
            _discountService.CreateProductGroupDiscount(discount);

            // Assert
            Assert.AreEqual(1, _dataStore.ProductGroupDiscounts.Count);
            var createdDiscount = _dataStore.ProductGroupDiscounts[0];
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
                Price = 9.99m
            };
            _dataStore.Products.Add(product);

            var discount = new ProductGroupDiscount
            {
                ProductId = product.Id,
                DiscountName = "Example Discount",
                Price = 19.99m,
                ProductCount = 5
            };

            // Act
            _discountService.CreateProductGroupDiscount(discount);

            // Assert
            Assert.AreEqual(1, _dataStore.ProductGroupDiscounts.Count);
            var createdDiscount = _dataStore.ProductGroupDiscounts[0];
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
                Price = 19.99m,
                ProductCount = 5
            };

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _discountService.CreateProductGroupDiscount(discount));
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
                Price = 9.99m
            };
            _dataStore.Products.Add(product);

            var existingDiscount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Existing Discount",
                Price = 19.99m,
                ProductCount = 5
            };
            _dataStore.ProductGroupDiscounts.Add(existingDiscount);

            var updatedDiscount = new ProductGroupDiscount
            {
                Id = existingDiscount.Id,
                ProductId = product.Id,
                DiscountName = "Updated Discount",
                Price = 29.99m,
                ProductCount = 10
            };

            // Act
            _discountService.UpdateProductGroupDiscount(updatedDiscount);

            // Assert
            var result = _dataStore.ProductGroupDiscounts.FirstOrDefault(d => d.Id == existingDiscount.Id);
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
                Price = 29.99m,
                ProductCount = 10
            };

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _discountService.UpdateProductGroupDiscount(updatedDiscount));
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
                Price = 19.99m,
                ProductCount = 5
            };
            _dataStore.ProductGroupDiscounts.Add(existingDiscount);

            var updatedDiscount = new ProductGroupDiscount
            {
                Id = existingDiscount.Id,
                ProductId = 2, // Non-existent product ID
                DiscountName = "Updated Discount",
                Price = 29.99m,
                ProductCount = 10
            };

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _discountService.UpdateProductGroupDiscount(updatedDiscount));
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
                Price = 19.99m,
                ProductCount = 5
            };
            _dataStore.ProductGroupDiscounts.Add(discount);

            // Act
            var result = _discountService.ReadProductGroupDiscount(1);

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
                Price = 9.99m
            };
            _dataStore.Products.Add(product);

            var existingDiscount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Existing Discount",
                Price = 19.99m,
                ProductCount = 5
            };
            _dataStore.ProductGroupDiscounts.Add(existingDiscount);

            // Act
            _discountService.DeleteProductGroupDiscount(existingDiscount.Id);

            // Assert
            Assert.AreEqual(0, _dataStore.ProductGroupDiscounts.Count);
        }

        [Test]
        public void DeleteProductGroupDiscount_ShouldThrowException_WhenDiscountDoesNotExist()
        {
            // Arrange
            var discountId = 1; // Non-existent discount ID

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _discountService.DeleteProductGroupDiscount(discountId));
        }

        [Test]
        public void GetApplicableDiscounts_ShouldReturnMatchingDiscounts_WhenProductAndCountMatch()
        {
            // Arrange
            var discount1 = new ProductGroupDiscount
            {
                ProductId = 1,
                ProductCount = 3,
                Price = 130
            };
            var discount2 = new ProductGroupDiscount
            {
                ProductId = 1,
                ProductCount = 5,
                Price = 200
            };
            _dataStore.ProductGroupDiscounts.AddRange(new List<ProductGroupDiscount> { discount1, discount2 });

            // Act
            var discounts = _discountService.GetApplicableDiscounts(1, 3);

            // Assert
            Assert.IsNotNull(discounts);
            Assert.AreEqual(1, discounts.Count);
            Assert.Contains(discount1, discounts);
        }

        [Test]
        public void GetApplicableDiscounts_ShouldReturnEmptyList_WhenNoMatchingDiscountsExist()
        {
            // Arrange
            var discount = new ProductGroupDiscount
            {
                ProductId = 1,
                ProductCount = 3,
                Price = 130
            };
            _dataStore.ProductGroupDiscounts.Add(discount);

            // Act
            var discounts = _discountService.GetApplicableDiscounts(2, 3);

            // Assert
            Assert.IsNotNull(discounts);
            Assert.AreEqual(0, discounts.Count);
        }

        [Test]
        public void GetApplicableDiscounts_ShouldReturnMultipleMatchingDiscounts_WhenProductAndCountMatchMultipleDiscounts()
        {
            // Arrange
            var discount1 = new ProductGroupDiscount
            {
                ProductId = 1,
                ProductCount = 2,
                Price = 45
            };
            var discount2 = new ProductGroupDiscount
            {
                ProductId = 1,
                ProductCount = 3,
                Price = 60
            };
            _dataStore.ProductGroupDiscounts.AddRange(new List<ProductGroupDiscount> { discount1, discount2 });

            // Act
            var discounts = _discountService.GetApplicableDiscounts(1, 3);

            // Assert
            Assert.IsNotNull(discounts);
            Assert.AreEqual(2, discounts.Count);
            Assert.Contains(discount1, discounts);
            Assert.Contains(discount2, discounts);
        }

    }

}
