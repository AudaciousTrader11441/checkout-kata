using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout
{
    public interface ICheckout
    {
        Cart CreateCart();
        Cart GetCart(int cartId);
        void RemoveProductFromCart(int cartId, int productId);
        void Scan(string item);
        decimal GetTotalPrice();
    }

    public class CheckoutCart : ICheckout
    {
        public Cart CurrentCart { get; set; }
        private IDataStore DataStore { get; set; }
        private List<IDiscount> discounts { get; set; }
        private IProductService ProductService { get; set; }

        private IProductGroupDiscountService ProductGroupDiscountService { get; set; }
        private int itemCount { get; set; }

        public CheckoutCart(IDataStore dataStoreIn, IProductService productService, IProductGroupDiscountService productGroupDiscountService)
        {
            CurrentCart = new Cart();
            discounts = new List<IDiscount>();
            ProductService = productService;
            DataStore = dataStoreIn;
            ProductGroupDiscountService = productGroupDiscountService;
            itemCount = 1;
        }

        private int GenerateNewCartId()
        {
            if (DataStore.Carts.Count > 0)
            {
                int maxId = DataStore.Carts.Max(c => c.Id);
                return maxId + 1;
            }
            else
            {
                return 1;
            }
        }
        public Cart CreateCart()
        {
            var cart = new Cart
            {
                Id = GenerateNewCartId(),
                Products = new List<CartProduct>()
            };

            DataStore.Carts.Add(cart);
            CurrentCart=cart;
            return cart;
        }

        public Cart GetCart(int cartId)
        {
            return DataStore.Carts.FirstOrDefault(c => c.Id == cartId);
        }


        public void Scan(string itemCode)
        {
            try
            {
                var product = ProductService.ReadProductBySKU(itemCode);
                if (product != null)
                {
                    var cart = GetCart(CurrentCart.Id);
                    var item = cart.Products.FirstOrDefault(e => e.ProductId == product.Id);
                    if (item == null)
                    {
                        cart.Products.Add(new CartProduct()
                        {
                            CartId = cart.Id,
                            Count = 1,
                            ProducePrice = product.Price,
                            ProductId = product.Id
                        });
                    }
                    else
                    {
                        item.Count++;
                    }
                }
                else
                {
                    throw new Exception("Product not found. Please contact store assistant.");
                }
            }
            catch (ArgumentException ex)
            {
                // Catch the ArgumentException thrown by ReadProductBySKU and rethrow as a more specific exception
                throw new Exception("Error scanning product: " + ex.Message, ex);
            }
        }

        public void LoadProductGroupDiscount()
        {
            foreach (var item in CurrentCart.Products)
            {
                var product = ProductService.ReadProduct(item.ProductId);

                if (product != null)
                {
                    var productDiscounts = ProductGroupDiscountService.GetApplicableDiscounts(item.ProductId, item.Count)
                        .OfType<ProductGroupDiscount>()
                        .OrderByDescending(d => d.ProductCount)
                        .ToList();

                    if (productDiscounts.Any())
                    {
                        decimal remainingCount = item.Count;
                        decimal totalDiscountedPrice = 0;

                        foreach (var discount in productDiscounts)
                        {
                            int discountMultiplier = (int)(remainingCount / discount.ProductCount);
                            if(discountMultiplier>0) discounts.Add(discount);//add in all the discounts that are applied on the cart items.
                            decimal discountedPrice = discountMultiplier * discount.Price;
                            remainingCount -= discountMultiplier * discount.ProductCount;
                            totalDiscountedPrice += discountedPrice;
                        }

                        decimal remainingPrice = remainingCount * product.Price;
                        item.ProducePrice = totalDiscountedPrice + remainingPrice;
                    }
                    else
                    {
                        item.ProducePrice = item.Count * product.Price;
                    }
                }
            }
        }

        public void RemoveProductFromCart(int cartId, int productId)
        {
            var cart = GetCart(cartId);
            if (cart != null)
            {
                var product = cart.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    cart.Products.Remove(product);
                }
                else
                {
                    throw new ArgumentException("Product does not exist in the cart.");
                }
            }
            else
            {
                throw new ArgumentException("Cart does not exist.");
            }
        }
        public decimal GetTotalPrice()
        {
            LoadProductGroupDiscount();
            decimal totalCost = CurrentCart.Products.Sum(e => e.ProducePrice);
            return totalCost;
        }
    }
}