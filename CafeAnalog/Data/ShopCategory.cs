namespace CafeAnalog.Data;

public class ShopCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    // Navigation properties

    public ICollection<ShopItem> Items { get; set; } = null!;
}
