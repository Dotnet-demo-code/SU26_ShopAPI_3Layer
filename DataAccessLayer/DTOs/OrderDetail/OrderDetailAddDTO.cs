namespace DataAccessLayer.DTOs.OrderDetail;

public class OrderDetailAddDTO
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
