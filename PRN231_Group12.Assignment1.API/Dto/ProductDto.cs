namespace PRN231_Group12.Assignment1.API.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public string? Weight { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
}

public class ProductDetailDto
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public string? Weight { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
    public string? CategoryName { get; set; }
}