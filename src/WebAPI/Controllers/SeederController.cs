using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Bogus;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/seeder")]
public class SeederController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IDataSeeder _seeder;

    public SeederController(ApplicationDbContext context, IDataSeeder seeder)
    {
        _context = context;
        _seeder = seeder;
    }

    [HttpPost]
    public async Task<IResult> Seed()
    {
        await _seeder.SeedAsync();
        return Results.Ok();
    }
}
