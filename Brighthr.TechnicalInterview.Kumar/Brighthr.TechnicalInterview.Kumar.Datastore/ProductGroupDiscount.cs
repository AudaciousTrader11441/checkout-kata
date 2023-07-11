namespace Brighthr.TechnicalInterview.Kumar.DataStore;

public class ProductGroupDiscount:IDiscount
{
    public int Id { get; set; }
    public int ProductId { get; set;}
    public string DiscountName { get; set; }
    public decimal Price { get; set; }
    public int ProductCount { get; set; }

}