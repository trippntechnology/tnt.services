using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Controllers;

[Authorize]
public class AnalyticsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Analytics
    public async Task<IActionResult> Index(string? eventType = null)
    {
        var query = _context.Analytics.AsQueryable();

        // Apply event type filter if provided
        if (!string.IsNullOrEmpty(eventType))
        {
            query = query.Where(a => a.EventType == eventType);
        }

        var analytics = await query.OrderByDescending(a => a.Timestamp).ToListAsync();

        // Get distinct event types for the filter dropdown
        var eventTypes = await _context.Analytics
            .Select(a => a.EventType)
            .Distinct()
            .OrderBy(e => e)
            .ToListAsync();

        // Pass event types and selected filter to view
        ViewBag.EventTypes = eventTypes;
        ViewBag.SelectedEventType = eventType;

        return View(analytics);
    }

    // GET: Analytics/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var analytic = await _context.Analytics
            .FirstOrDefaultAsync(m => m.Id == id);
        if (analytic == null)
        {
            return NotFound();
        }

        return View(analytic);
    }
}
