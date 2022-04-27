using CafeAnalog.Data;
using CafeAnalog.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeAnalog.Controllers;

public class HomeController : Controller
{
    
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Leaderboard()
    {
        var users = await _dbContext.Users.ToListAsync();

        var rankings = users.Select(u =>
                new RankingModel {Score = u.Score, FirstName = u.FirstName, LastName = u.LastName})
            .OrderByDescending(r => r.Score)
            .ToList();
        
        return View(rankings);
    }
}
