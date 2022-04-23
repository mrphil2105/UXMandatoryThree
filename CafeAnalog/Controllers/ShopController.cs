using CafeAnalog.Data;
using CafeAnalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Controllers;

public class ShopController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public ShopController(AppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _dbContext.ShopCategories.Include(c => c.Items)
            .ToListAsync();

        var categoryModels = categories.Select(c => new ShopCategoryModel
            {
                Name = c.Name,
                Items = c.Items.Select(i => new ShopItemModel { Id = i.Id, Name = i.Name, Price = i.Price })
                    .ToList()
            })
            .ToList();

        return View(categoryModels);
    }

    // Endpoint that redirects a user to the shop page after log in (because return url is /Shop/Buy).
    [Authorize]
    public IActionResult Buy()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Buy([FromBody] int itemId)
    {
        var user = await _userManager.GetUserAsync(User);
        var item = await _dbContext.ShopItems.FindAsync(itemId);

        if (item == null)
        {
            return BadRequest();
        }

        if (user.Balance < item.Price)
        {
            return Content("You have insufficient funds to purchase this item.");
        }

        var ticket = await _dbContext.InventoryTickets.SingleOrDefaultAsync(t => t.ItemId == itemId) ??
            new InventoryTicket { ItemId = itemId, User = user };
        ticket.Count += 10;

        user.Balance -= item.Price;

        _dbContext.InventoryTickets.Update(ticket);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Index", "Inventory");
    }
}
