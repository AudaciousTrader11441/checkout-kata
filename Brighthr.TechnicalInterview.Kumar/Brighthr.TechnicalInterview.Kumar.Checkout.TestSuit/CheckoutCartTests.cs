using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout.TestSuit
{
    [TestFixture]
    public class CheckoutCartTests
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
        public void CreateCart_ShouldCreateNewCart()
        {
            // Act
            var cart = _checkout.CreateCart();

            // Assert
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.Id);
            Assert.AreEqual(0, cart.Products.Count);
            Assert.AreEqual(1, _dataStore.Carts.Count);
            Assert.AreEqual(cart, _dataStore.Carts[0]);
        }

        [Test]
        public void GetCart_ShouldReturnExistingCart()
        {
            // Arrange
            var cart = new Cart { Id = 1 };
            _dataStore.Carts.Add(cart);

            // Act
            var result = _checkout.GetCart(1);

            // Assert
            Assert.AreEqual(cart, result);
        }

        [Test]
        public void GetCart_ShouldReturnNull_WhenCartDoesNotExist()
        {
            // Act
            var result = _checkout.GetCart(1);

            // Assert
            Assert.IsNull(result);
        }


        [Test]
        public void Scan_ShouldAddProductToCart_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };
            _dataStore.Products.Add(product);
            var cart = _checkout.CreateCart();

            // Act
            _checkout.Scan("ABC123");

            // Assert
            Assert.AreEqual(1, cart.Products.Count);
            var cartProduct = cart.Products[0];
            Assert.AreEqual(1, cartProduct.Count);
            Assert.AreEqual(product.Price, cartProduct.ProducePrice);
            Assert.AreEqual(product.Id, cartProduct.ProductId);
        }


        [Test]
        public void Scan_ShouldIncreaseProductCountInCart_WhenProductAlreadyExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };
            _dataStore.Products.Add(product);
            var cart = _checkout.CreateCart();
            _checkout.Scan("ABC123");

            // Act
            _checkout.Scan("ABC123");

            // Assert
            Assert.AreEqual(1, cart.Products.Count);
            var cartProduct = cart.Products[0];
            Assert.AreEqual(2, cartProduct.Count);
            Assert.AreEqual(product.Price, cartProduct.ProducePrice);
            Assert.AreEqual(product.Id, cartProduct.ProductId);
        }

        [Test]
        public void Scan_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Act and Assert
            Assert.Throws<Exception>(() => _checkout.Scan("XYZ789"));
        }

        [Test]
        public void RemoveProductFromCart_ShouldRemoveProduct_WhenProductExistsInCart()
        {
            // Arrange
            var cart = new Cart { Id = 1 };
            var product = new CartProduct { ProductId = 1 };
            cart.Products.Add(product);
            _dataStore.Carts.Add(cart);

            // Act
            _checkout.RemoveProductFromCart(cart.Id, product.ProductId);

            // Assert
            Assert.AreEqual(0, cart.Products.Count);
        }

        [Test]
        public void RemoveProductFromCart_ShouldThrowException_WhenCartDoesNotExist()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _checkout.RemoveProductFromCart(1, 1));
        }

        [Test]
        public void RemoveProductFromCart_ShouldThrowException_WhenProductDoesNotExistInCart()
        {
            // Arrange
            var cart = new Cart { Id = 1 };
            _dataStore.Carts.Add(cart);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _checkout.RemoveProductFromCart(cart.Id, 1));
        }

        
    }

}