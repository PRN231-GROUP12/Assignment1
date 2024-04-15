namespace PRN231_Group12.Assignment1.API.Request;

public class UpdateOrderRequest
{
    public int MemberId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public DateTime ShippedDate { get; set; }
    public decimal Freight { get; set; }
}