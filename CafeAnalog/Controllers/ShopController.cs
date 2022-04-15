using CafeAnalog.Data;
using CafeAnalog.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Controllers;

public class ShopController : Controller
{
    private readonly AppDbContext _dbContext;

    public ShopController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
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
}
