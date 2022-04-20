using CafeAnalog.Data;
using CafeAnalog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Controllers;

[Authorize]
public class InventoryController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public InventoryController(AppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _dbContext.InventoryTickets.Include(t => t.Item)
            .ToListAsync();

        var ticketModels = tickets
            .Select(t => new InventoryTicketModel { Id = t.Id, Name = t.Item.Name, Count = t.Count })
            .ToList();

        return View(ticketModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UseTicket([FromBody] int ticketId)
    {
        var user = await _userManager.GetUserAsync(User);
        var ticket = await _dbContext.InventoryTickets.Include(t => t.User)
            .SingleOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null || ticket.User.Id != user.Id)
        {
            return BadRequest();
        }

        ticket.Count--;

        if (ticket.Count == 0)
        {
            _dbContext.InventoryTickets.Remove(ticket);
        }

        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
