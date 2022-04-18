using CafeAnalog.Data;
using CafeAnalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Controllers;

[Authorize]
public class InventoryController : Controller
{
    private readonly AppDbContext _dbContext;

    public InventoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _dbContext.InventoryTickets.Include(t => t.Item)
            .ToListAsync();

        var ticketModels = tickets.Select(t => new InventoryTicketModel { Name = t.Item.Name, Count = t.Count })
            .ToList();

        return View(ticketModels);
    }
}
