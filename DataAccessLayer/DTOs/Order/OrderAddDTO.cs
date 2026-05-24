namespace DataAccessLayer.DTOs.Order;

public class OrderAddDTO
{
    public int UserId { get; set; }

    public DateOnly OrderDate { get; set; }
}
