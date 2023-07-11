namespace Brighthr.TechnicalInterview.Kumar.DataStore;

public class Cart
{
    public Cart()
    {
        Products = new List<CartProduct>();
    }
    public int Id { get; set; }

    public List<CartProduct> Products { get; set; }
        
}