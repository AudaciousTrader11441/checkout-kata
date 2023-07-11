using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout.TestSuit
{
    [TestFixture]
    public class ProductGroupDiscountTest
    {
        private IDataStore _dataStore;
        private IProductService _productService;
        private IProductGroupDiscountService _productGroupDiscountService;
        private CheckoutCart _checkout;

        [SetUp]
        public void Setup()
        {
            _dataStore = new InMemory();
            _productService = new ProductService(_dataStore);
            _productGroupDiscountService = new ProductGroupDiscountService(_dataStore);
            _checkout = new CheckoutCart(_dataStore, _productService, _productGroupDiscountService);
        }

        [Test]
        public void LoadProductGroupDiscount_ShouldSetProducePriceToRegularPrice_WhenNoDiscounts()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };

            var cartProduct = new CartProduct
            {
                ProductId = product.Id,
                Count = 3,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product);
            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(29.97f, cartProduct.ProducePrice); // Regular price: 3 * 9.99 = 29.97
        }


        [Test]
        public void LoadProductGroupDiscount_ShouldApplySingleDiscount_WhenOneDiscountExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };

            var discount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Discount 1",
                Price = 20m,
                ProductCount = 3
            };

            var cartProduct = new CartProduct
            {
                ProductId = product.Id,
                Count = 5,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product);
            _dataStore.ProductGroupDiscounts.Add(discount);
            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(39.98f, cartProduct.ProducePrice); // Discounted price: (1 * 20) + (2 * 9.99) = 39.98
        }


        [Test]
        public void LoadProductGroupDiscount_ShouldApplyMultipleDiscounts_WhenMultipleDiscountsExist()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };

            var discount1 = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Discount 1",
                Price = 20m,
                ProductCount = 3
            };

            var discount2 = new ProductGroupDiscount
            {
                Id = 2,
                ProductId = product.Id,
                DiscountName = "Discount 2",
                Price = 30m,
                ProductCount = 5
            };

            var cartProduct = new CartProduct
            {
                ProductId = product.Id,
                Count = 9,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product);
            _dataStore.ProductGroupDiscounts.Add(discount1);
            _dataStore.ProductGroupDiscounts.Add(discount2);
            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.That(cartProduct.ProducePrice, Is.EqualTo(59.99m)); // Discounted price: (1 * 30) + (1 * 20) + (1 * 9.99) = 69.99
        }


        [Test]
        public void LoadProductGroupDiscount_ShouldApplyMultipleDiscountsAndRemainingItems_WhenMultipleDiscountsExist()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };

            var discount1 = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Discount 1",
                Price = 20m,
                ProductCount = 3
            };

            var discount2 = new ProductGroupDiscount
            {
                Id = 2,
                ProductId = product.Id,
                DiscountName = "Discount 2",
                Price = 30m,
                ProductCount = 5
            };

            var cartProduct = new CartProduct
            {
                ProductId = product.Id,
                Count = 14,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product);
            _dataStore.ProductGroupDiscounts.Add(discount1);
            _dataStore.ProductGroupDiscounts.Add(discount2);
            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(89.99m, cartProduct.ProducePrice); // Discounted price: (2 * 30) + (1 * 20) + (1 * 9.99) = 89.99
        }


        [Test]
        public void LoadProductGroupDiscount_ShouldApplyRemainingItemsToRegularPrice_WhenNoBestFitDiscount()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };

            var discount1 = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product.Id,
                DiscountName = "Discount 1",
                Price = 20m,
                ProductCount = 5
            };

            var discount2 = new ProductGroupDiscount
            {
                Id = 2,
                ProductId = product.Id,
                DiscountName = "Discount 2",
                Price = 30m,
                ProductCount = 5
            };

            var cartProduct = new CartProduct
            {
                ProductId = product.Id,
                Count = 4,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product);
            _dataStore.ProductGroupDiscounts.Add(discount1);
            _dataStore.ProductGroupDiscounts.Add(discount2);
            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(39.96f, cartProduct.ProducePrice); // Discounted price: (4 * 9.99) = 39.96
        }


        [Test]

        public void LoadProductGroupDiscount_ShouldSetProducePriceToRegularPrice_WhenNoDiscountsAndInvalidProductId()
        {
            // Arrange
            var cartProduct = new CartProduct
            {
                ProductId = 10, // Invalid product ID
                Count = 3,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(0m, cartProduct.ProducePrice); // Regular price: 0 (since invalid product ID)
        }
        [Test]
        public void LoadProductGroupDiscount_ShouldSetProducePriceToRegularPrice_WhenOneDiscountAndInvalidProductId()
        {
            // Arrange
            var discount = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = 1,
                DiscountName = "Discount 1",
                Price = 20m,
                ProductCount = 3
            };

            var cartProduct = new CartProduct
            {
                ProductId = 10, // Invalid product ID
                Count = 6,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.ProductGroupDiscounts.Add(discount);
            _checkout.CurrentCart.Products.Add(cartProduct);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(0m, cartProduct.ProducePrice); // Regular price: 0 (since invalid product ID)
        }
        [Test]
        public void LoadProductGroupDiscount_ShouldNotSetProducePrice_WhenCartIsEmpty()
        {
            // Arrange
            var cart = _checkout.CreateCart();

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.IsEmpty(cart.Products); // Cart should remain empty
        }
        [Test]
        public void LoadProductGroupDiscount_ShouldSetProducePriceToRegularPrice_WhenNoApplicableDiscounts()
        {
            // Arrange
            var product1 = new Product
            {
                Id = 1,
                Name = "Product 1",
                SKU = "ABC123",
                Price = 9.99m
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Product 2",
                SKU = "DEF456",
                Price = 19.99m
            };

            var cartProduct1 = new CartProduct
            {
                ProductId = product1.Id,
                Count = 3,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            var cartProduct2 = new CartProduct
            {
                ProductId = product2.Id,
                Count = 2,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product1);
            _dataStore.Products.Add(product2);
            _checkout.CurrentCart.Products.Add(cartProduct1);
            _checkout.CurrentCart.Products.Add(cartProduct2);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(29.97f, cartProduct1.ProducePrice); // Regular price: 3 * 9.99 = 29.97
            Assert.AreEqual(39.98f, cartProduct2.ProducePrice); // Regular price: 2 * 19.99 = 39.98
        }

        [Test]
        public void LoadProductGroupDiscount_ShouldApplyMultipleDiscountsForMultipleProduct_WhenMultipleDiscountsExist()
        {
            // Arrange
            var product1 = new Product
            {
                Id = 1,
                Name = "Product 1",
                SKU = "ABC123",
                Price = 9.99m
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Product 2",
                SKU = "DEF456",
                Price = 19.99m
            };

            var discount1 = new ProductGroupDiscount
            {
                Id = 1,
                ProductId = product1.Id,
                DiscountName = "Discount 1",
                Price = 5.0m,
                ProductCount = 2
            };

            var discount2 = new ProductGroupDiscount
            {
                Id = 2,
                ProductId = product2.Id,
                DiscountName = "Discount 2",
                Price = 10.0m,
                ProductCount = 3
            };

            var cartProduct1 = new CartProduct
            {
                ProductId = product1.Id,
                Count = 5,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            var cartProduct2 = new CartProduct
            {
                ProductId = product2.Id,
                Count = 7,
                ProducePrice = 0m // Initialize to 0 for testing
            };

            _dataStore.Products.Add(product1);
            _dataStore.Products.Add(product2);
            _dataStore.ProductGroupDiscounts.Add(discount1);
            _dataStore.ProductGroupDiscounts.Add(discount2);
            _checkout.CurrentCart.Products.Add(cartProduct1);
            _checkout.CurrentCart.Products.Add(cartProduct2);

            // Act
            _checkout.LoadProductGroupDiscount();

            // Assert
            Assert.AreEqual(19.99m, cartProduct1.ProducePrice); // Discounted price: 2 * 5.0 + 1 * 9.99 =  19.99
            Assert.AreEqual(39.99m, cartProduct2.ProducePrice); // Discounted price: 2 * 10.0 + 1 * 19.99 = 39.99
        }



    }

}
