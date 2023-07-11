namespace Brighthr.TechnicalInterview.Kumar.DataStore;

public class CartProduct
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CartId { get; set; }
    public int Count { get; set; }
    public decimal ProducePrice { get; set; }
}