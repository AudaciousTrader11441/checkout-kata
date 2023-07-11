using Brighthr.TechnicalInterview.Kumar.Checkout;

namespace Brighthr.TechnicalInterview.Kumar.DataStore.TestSuit
{


    [TestFixture]
    public class ProductServiceTests
    {
        private IDataStore dataStore;
        private IProductService productService;

        [SetUp]
        public void Initialize()
        {
            dataStore = new InMemory();
            productService = new ProductService(dataStore);
        }

        [Test]
        public void CreateProduct_ShouldAddNewProductToDataStore()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99m
            };

            // Act
            productService.CreateProduct(product);

            // Assert
            Assert.AreEqual(1, dataStore.Products.Count);
            Assert.AreEqual(product, dataStore.Products[0]);
        }

        [Test]
        public void ReadProduct_ShouldReturnExistingProductFromDataStore()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99m
            };
            dataStore.Products.Add(product);

            // Act
            var result = productService.ReadProduct(1);

            // Assert
            Assert.AreEqual(product, result);
        }

        [Test]
        public void UpdateProduct_ShouldUpdateExistingProductInDataStore()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99m
            };
            dataStore.Products.Add(product);

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Product",
                SKU = "SKU002",
                Price = 19.99m
            };

            // Act
            productService.UpdateProduct(updatedProduct);
            var result = dataStore.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedProduct.Name, result.Name);
            Assert.AreEqual(updatedProduct.SKU, result.SKU);
            Assert.AreEqual(updatedProduct.Price, result.Price);
        }

        [Test]
        public void DeleteProduct_ShouldRemoveProductFromDataStore()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "SKU001",
                Price = 9.99m
            };
            dataStore.Products.Add(product);

            // Act
            productService.DeleteProduct(1);
            var result = dataStore.Products.FirstOrDefault(p => p.Id == 1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void ReadProductBySKU_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Example Product",
                SKU = "ABC123",
                Price = 9.99m
            };
            dataStore.Products.Add(product);

            // Act
            var result = productService.ReadProductBySKU("ABC123");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(product.Name, result.Name);
            Assert.AreEqual(product.SKU, result.SKU);
            Assert.AreEqual(product.Price, result.Price);
        }

        [Test]
        public void ReadProductBySKU_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange - No products added to the data store

            // Act and Assert
            Assert.Throws<ArgumentException>(() => productService.ReadProductBySKU("XYZ789"));
        }

    }


}