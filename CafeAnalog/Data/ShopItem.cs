namespace CafeAnalog.Data;

public class ShopItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    // Navigation properties

    public ShopCategory Category { get; set; } = null!;
}
