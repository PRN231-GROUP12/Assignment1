namespace PRN231_Group12.Assignment1.API.DTO;

public class OrderDetailDto
{
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public double Discount { get; set; }
}