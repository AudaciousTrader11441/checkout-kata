namespace Brighthr.TechnicalInterview.Kumar.DataStore
{
    public class InMemory: IDataStore
    {
        public InMemory()
        {
            Products = new List<Product>();
            ProductGroupDiscounts = new List<ProductGroupDiscount>();
            Carts = new List<Cart>();
        }
        public List<Product> Products { get; set; }
        public List<ProductGroupDiscount> ProductGroupDiscounts { get; set; }
        public List<Cart> Carts { get; set; }

    }
}