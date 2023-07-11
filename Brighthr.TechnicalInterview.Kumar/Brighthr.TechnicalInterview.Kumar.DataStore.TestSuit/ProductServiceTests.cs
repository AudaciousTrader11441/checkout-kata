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
                Price = 9.99f
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
                Price = 9.99f
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
                Price = 9.99f
            };
            dataStore.Products.Add(product);

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Product",
                SKU = "SKU002",
                Price = 19.99f
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
                Price = 9.99f
            };
            dataStore.Products.Add(product);

            // Act
            productService.DeleteProduct(1);
            var result = dataStore.Products.FirstOrDefault(p => p.Id == 1);

            // Assert
            Assert.IsNull(result);
        }
    }


}