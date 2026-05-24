namespace DataAccessLayer.DTOs.Order;

public class OrderDTO
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateOnly OrderDate { get; set; }
}
