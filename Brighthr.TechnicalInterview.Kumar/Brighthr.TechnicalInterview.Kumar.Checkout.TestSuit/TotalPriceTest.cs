using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout.TestSuit
{


    [TestFixture]
    public class CheckoutTotalPriceTestTests
    {
        private IDataStore _dataStore;
        private IProductService _productService;
        private IProductGroupDiscountService _productGroupDiscountService;
        private ICheckout _checkout;

        [SetUp]
        public void Setup()
        {
            _dataStore = new InMemory();
            _productService = new ProductService(_dataStore);
            _productGroupDiscountService = new ProductGroupDiscountService(_dataStore);
            _checkout = new CheckoutCart(_dataStore, _productService, _productGroupDiscountService);
            var product1 = new Product { Id = 1, Name = "Product A", SKU = "A", Price = 50m };
            var product2 = new Product { Id = 2, Name = "Product B", SKU = "B", Price = 30m };
            var product3 = new Product { Id = 3, Name = "Product C", SKU = "C", Price = 20m };
            var product4 = new Product { Id = 4, Name = "Product D", SKU = "D", Price = 15m };
            _productService.CreateProduct(product1);
            _productService.CreateProduct(product2);
            _productService.CreateProduct(product3);
            _productService.CreateProduct(product4);
        }

        [Test]
        public void GetTotalPrice_ShouldReturnZero_WhenCartIsEmpty()
        {
            // Arrange
            var cart = _checkout.CreateCart();

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(0m, totalPrice);
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithoutDiscounts_WhenNoDiscountsExist()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            _checkout.Scan("A");
            _checkout.Scan("B");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(80m, totalPrice); // 50 + 30 = 80
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenDiscountsExist()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("B");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(190m, totalPrice); // Discounted price for 3 A's = 130
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithMultipleDiscounts_WhenMultipleDiscountsExist()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _productGroupDiscountService.CreateProductGroupDiscount(discount2);
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("B");
            _checkout.Scan("B");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(205m, totalPrice); // Discounted price for 3 A's = 130, Discounted price for 2 B's = 45, Total = 205
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithNonDiscountedAndDiscountedItems_WhenMixedProductsScanned()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _productGroupDiscountService.CreateProductGroupDiscount(discount2);
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("C");
            _checkout.Scan("B");
            _checkout.Scan("D");
            _checkout.Scan("D");
            _checkout.Scan("A");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(225m, totalPrice); //Total = 195
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithNonDiscounted_WhenMixedProductsScanned()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 5 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 6 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _productGroupDiscountService.CreateProductGroupDiscount(discount2);
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("C");
            _checkout.Scan("B");
            _checkout.Scan("D");
            _checkout.Scan("D");
            _checkout.Scan("A");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(260m, totalPrice); //Total = 260
        }

        [Test]
        public void GetTotalPrice_ShouldReturnZero_WhenNoProductsScanned()
        {
            // Arrange
            var cart = _checkout.CreateCart();

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(0m, totalPrice);
        }

        [Test]
        public void GetTotalPrice_ShouldReturnZero_WhenInvalidProductScanned()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var product = new Product { Id = 1, Name = "Product A", Price = 50m };
            _productService.CreateProduct(product);
            _checkout.Scan("A");

            // Act and Assert
            Assert.Throws<Exception>(() => _checkout.Scan("E"));// Scanning an invalid product

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(50m, totalPrice); // Only valid product A should contribute to the total price
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenDiscountsOverlap()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var product = new Product { Id = 1, Name = "Product A", Price = 50m };
            _productService.CreateProduct(product);
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 1, Price = 90m, ProductCount = 2 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _productGroupDiscountService.CreateProductGroupDiscount(discount2);
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(180m, totalPrice); // Discounted price for 3 A's (130) + Regular price for 1 A (50) = 180
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenMultipleDiscountsExistForDifferentProducts()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var product1 = new Product { Id = 1, Name = "Product A", Price = 50m };
            var product2 = new Product { Id = 2, Name = "Product B", Price = 30m };
            _productService.CreateProduct(product1);
            _productService.CreateProduct(product2);
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _productGroupDiscountService.CreateProductGroupDiscount(discount2);
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("B");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(175m, totalPrice); // Discounted price for 3 A's (130) + Discounted price for 2 B's (45) = 175
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenMixedProductCountAndDiscounts()
        {
            // Arrange
            var cart = _checkout.CreateCart();
            var product1 = new Product { Id = 1, Name = "Product A", Price = 50m };
            var product2 = new Product { Id = 2, Name = "Product B", Price = 30m };
            var product3 = new Product { Id = 3, Name = "Product C", Price = 20m };
            _productService.CreateProduct(product1);
            _productService.CreateProduct(product2);
            _productService.CreateProduct(product3);
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            _productGroupDiscountService.CreateProductGroupDiscount(discount1);
            _productGroupDiscountService.CreateProductGroupDiscount(discount2);
            _checkout.Scan("A");
            _checkout.Scan("B");
            _checkout.Scan("A");
            _checkout.Scan("C");
            _checkout.Scan("B");
            _checkout.Scan("D");
            _checkout.Scan("A");

            // Act
            var totalPrice = _checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(210m, totalPrice);
        }
    }

}
