using Brighthr.TechnicalInterview.Kumar.DataStore;

namespace Brighthr.TechnicalInterview.Kumar.Checkout;

public interface ICheckout
{
    Cart CreateCart();
    Cart GetCart(int cartId);
    void RemoveProductFromCart(int cartId, int productId);
    void Scan(string item);
    decimal GetTotalPrice();
}