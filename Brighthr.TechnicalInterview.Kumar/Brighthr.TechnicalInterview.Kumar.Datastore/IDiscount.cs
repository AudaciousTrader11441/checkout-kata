namespace Brighthr.TechnicalInterview.Kumar.DataStore;

public interface IDiscount
{
    public int Id { get; set; }
    public string DiscountName { get; set; }
    public decimal Price { get; set; }

}