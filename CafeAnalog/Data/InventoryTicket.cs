namespace CafeAnalog.Data;

public class InventoryTicket
{
    public int Id { get; set; }

    public int Count { get; set; }

    // Navigation properties

    public int ItemId { get; set; }

    public ShopItem Item { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public AppUser User { get; set; } = null!;
}
