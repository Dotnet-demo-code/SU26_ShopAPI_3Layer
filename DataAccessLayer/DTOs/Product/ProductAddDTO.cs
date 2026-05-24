namespace DataAccessLayer.DTOs.Product;

public class ProductAddDTO
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
