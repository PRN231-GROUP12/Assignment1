namespace PRN231_Group12.Assignment1.API.Request;

public class CreateProductRequest
{
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int CategoryId { get; set; }
    public int UnitsInStock { get; set; }
    public string Weight { get; set; } = null!;
}