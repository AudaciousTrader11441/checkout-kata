namespace Brighthr.TechnicalInterview.Kumar.DataStore
{
    public interface IDataStore
    {
        List<Product> Products { get; set; }
        List<ProductGroupDiscount> ProductGroupDiscounts { get; set; }
        List<Cart> Carts { get; set; }
    }

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


    public class Product
    { 
       public int Id { get; set; }
       public string Name { get; set; }
       public string SKU { get; set; }
       public float Price { get; set; }
    }

    public interface IDiscount
    {
        public int Id { get; set; }
        public string DiscountName { get; set; }
        public float Price { get; set; }

    }

    public class ProductGroupDiscount:IDiscount
    {
        public int Id { get; set; }
        public int ProductId { get; set;}
        public string DiscountName { get; set; }
        public float Price { get; set; }
        public int ProductCount { get; set; }

    }

    public class Cart
    {
        public int Id { get; set; }

        public List<CartProduct> Products { get; set; }
        
    }

    public class CartProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Count { get; set; }
        public float ProducePrice { get; set; }
    }
}