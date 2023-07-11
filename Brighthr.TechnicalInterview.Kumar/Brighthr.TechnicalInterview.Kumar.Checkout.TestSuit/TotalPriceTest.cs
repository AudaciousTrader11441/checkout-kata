using Brighthr.TechnicalInterview.Kumar.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brighthr.TechnicalInterview.Kumar.Checkout.TestSuit
{


    [TestFixture]
    public class CheckoutTotalPriceTestTests
    {
        private IDataStore dataStore;
        private IProductService productService;
        private IProductGroupDiscountService productGroupDiscountService;
        private ICheckout checkout;

        [SetUp]
        public void Setup()
        {
            dataStore = new InMemory();
            productService = new ProductService(dataStore);
            productGroupDiscountService = new ProductGroupDiscountService(dataStore);
            checkout = new CheckoutCart(dataStore, productService, productGroupDiscountService);
            var product1 = new Product { Id = 1, Name = "Product A", SKU = "A", Price = 50m };
            var product2 = new Product { Id = 2, Name = "Product B", SKU = "B", Price = 30m };
            var product3 = new Product { Id = 3, Name = "Product C", SKU = "C", Price = 20m };
            var product4 = new Product { Id = 4, Name = "Product D", SKU = "D", Price = 15m };
            productService.CreateProduct(product1);
            productService.CreateProduct(product2);
            productService.CreateProduct(product3);
            productService.CreateProduct(product4);
        }

        [Test]
        public void GetTotalPrice_ShouldReturnZero_WhenCartIsEmpty()
        {
            // Arrange
            var cart = checkout.CreateCart();

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(0m, totalPrice);
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithoutDiscounts_WhenNoDiscountsExist()
        {
            // Arrange
            var cart = checkout.CreateCart();
            checkout.Scan("A");
            checkout.Scan("B");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(80m, totalPrice); // 50 + 30 = 80
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenDiscountsExist()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("B");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(190m, totalPrice); // Discounted price for 3 A's = 130
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithMultipleDiscounts_WhenMultipleDiscountsExist()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            productGroupDiscountService.CreateProductGroupDiscount(discount2);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(205m, totalPrice); // Discounted price for 3 A's = 130, Discounted price for 2 B's = 45, Total = 205
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithNonDiscountedAndDiscountedItems_WhenMixedProductsScanned()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            productGroupDiscountService.CreateProductGroupDiscount(discount2);
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("A");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(225m, totalPrice); //Total = 195
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithNonDiscounted_WhenMixedProductsScanned()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 5 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 6 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            productGroupDiscountService.CreateProductGroupDiscount(discount2);
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("D");
            checkout.Scan("D");
            checkout.Scan("A");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(260m, totalPrice); //Total = 260
        }

        [Test]
        public void GetTotalPrice_ShouldReturnZero_WhenNoProductsScanned()
        {
            // Arrange
            var cart = checkout.CreateCart();

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(0m, totalPrice);
        }

        [Test]
        public void GetTotalPrice_ShouldReturnZero_WhenInvalidProductScanned()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var product = new Product { Id = 1, Name = "Product A", Price = 50m };
            productService.CreateProduct(product);
            checkout.Scan("A");

            // Act and Assert
            Assert.Throws<Exception>(() => checkout.Scan("E"));// Scanning an invalid product

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(50m, totalPrice); // Only valid product A should contribute to the total price
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenDiscountsOverlap()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var product = new Product { Id = 1, Name = "Product A", Price = 50m };
            productService.CreateProduct(product);
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 1, Price = 90m, ProductCount = 2 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            productGroupDiscountService.CreateProductGroupDiscount(discount2);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(180m, totalPrice); // Discounted price for 3 A's (130) + Regular price for 1 A (50) = 180
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenMultipleDiscountsExistForDifferentProducts()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var product1 = new Product { Id = 1, Name = "Product A", Price = 50m };
            var product2 = new Product { Id = 2, Name = "Product B", Price = 30m };
            productService.CreateProduct(product1);
            productService.CreateProduct(product2);
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            productGroupDiscountService.CreateProductGroupDiscount(discount2);
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("B");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(175m, totalPrice); // Discounted price for 3 A's (130) + Discounted price for 2 B's (45) = 175
        }

        [Test]
        public void GetTotalPrice_ShouldCalculateTotalWithDiscounts_WhenMixedProductCountAndDiscounts()
        {
            // Arrange
            var cart = checkout.CreateCart();
            var product1 = new Product { Id = 1, Name = "Product A", Price = 50m };
            var product2 = new Product { Id = 2, Name = "Product B", Price = 30m };
            var product3 = new Product { Id = 3, Name = "Product C", Price = 20m };
            productService.CreateProduct(product1);
            productService.CreateProduct(product2);
            productService.CreateProduct(product3);
            var discount1 = new ProductGroupDiscount { Id = 1, ProductId = 1, Price = 130m, ProductCount = 3 };
            var discount2 = new ProductGroupDiscount { Id = 2, ProductId = 2, Price = 45m, ProductCount = 2 };
            productGroupDiscountService.CreateProductGroupDiscount(discount1);
            productGroupDiscountService.CreateProductGroupDiscount(discount2);
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("C");
            checkout.Scan("B");
            checkout.Scan("D");
            checkout.Scan("A");

            // Act
            var totalPrice = checkout.GetTotalPrice();

            // Assert
            Assert.AreEqual(210m, totalPrice);
        }
    }

}
