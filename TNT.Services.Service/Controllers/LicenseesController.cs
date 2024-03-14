using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Controllers
{
  public class LicenseesController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<LicenseesController> _logger;

    public LicenseesController(ApplicationDbContext context, ILogger<LicenseesController> logger)
    {
      _context = context;
      _logger = logger;
    }

    private List<LicenseePlus> GetLicenseePlus(List<Licensee> licensees, List<Models.Entities.Application> applications)
    {
      return (from l in licensees
              join a in applications on l.ApplicationId equals a.ID
              select new LicenseePlus(l, a.Name))
              .OrderBy(l => l.ValidUntil)
              .ToList();
    }

    // GET: Licensees
    public IActionResult Index()
    {
      var applications = _context.Application.ToList();
      var licensees = _context.Licensee.ToList();
      return View(GetLicenseePlus(licensees, applications));
    }

    // GET: Licensees/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var licensee = await _context.Licensee
          .FirstOrDefaultAsync(m => m.ID == id);
      if (licensee == null)
      {
        return NotFound();
      }

      var application = await _context.Application.FindAsync(licensee.ApplicationId);
      var licenseePlus = new LicenseePlus(licensee, application?.Name ?? "");

      return View(licenseePlus);
    }

    // GET: Licensees/Create
    public IActionResult Create()
    {
      List<Application> applications = _context.Application.OrderBy(a => a.Name).ToList();
      ViewBag.Applications = new SelectList(applications, "ID", "Name");
      return View();
    }

    // POST: Licensees/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,Name,ApplicationId,ValidUntil")] Licensee licensee)
    {
      if (ModelState.IsValid)
      {
        _context.Add(licensee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(licensee);
    }

    // GET: Licensees/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var licensee = await _context.Licensee.FindAsync(id);
      if (licensee == null)
      {
        return NotFound();
      }

      //ViewBag.ApplicationId = licensee.ApplicationId;

      var application = await _context.Application.FindAsync(licensee.ApplicationId);
      if (application == null) return NotFound();

      return View(new LicenseePlus(licensee, application.Name));
    }

    // POST: Licensees/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID,Name,ApplicationId,ValidUntil,ApplicationName")] Licensee licensee)
    {
      if (id != licensee.ID)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(licensee);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!LicenseeExists(licensee.ID))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(licensee);
    }

    // GET: Licensees/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var licensee = await _context.Licensee
          .FirstOrDefaultAsync(m => m.ID == id);
      if (licensee == null)
      {
        return NotFound();
      }

      return View(licensee);
    }

    // POST: Licensees/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var licensee = await _context.Licensee.FindAsync(id);
      if (licensee != null)
      {
        _context.Licensee.Remove(licensee);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool LicenseeExists(int id)
    {
      return _context.Licensee.Any(e => e.ID == id);
    }
  }
}
