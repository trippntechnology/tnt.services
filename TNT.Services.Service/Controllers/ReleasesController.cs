using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace TNT.Update.Service.Controllers
{
	public class ReleasesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ReleasesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Releases
		public async Task<IActionResult> Index()
		{
			return View(await _context.Release.ToListAsync());
		}

		// GET: Releases/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var release = await _context.Release
					.FirstOrDefaultAsync(m => m.ID == id);
			if (release == null)
			{
				return NotFound();
			}

			return View(release);
		}

		// GET: Releases/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Releases/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,ApplicationID,Package")] Release release)
		{
			if (ModelState.IsValid)
			{
				_context.Add(release);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(release);
		}

		// GET: Releases/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var release = await _context.Release.FindAsync(id);
			if (release == null)
			{
				return NotFound();
			}
			return View(release);
		}

		// POST: Releases/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,ApplicationID,Package")] Release release)
		{
			if (id != release.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(release);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReleaseExists(release.ID))
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
			return View(release);
		}

		// GET: Releases/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var release = await _context.Release
					.FirstOrDefaultAsync(m => m.ID == id);
			if (release == null)
			{
				return NotFound();
			}

			return View(release);
		}

		// POST: Releases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var release = await _context.Release.FindAsync(id);
			_context.Release.Remove(release);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ReleaseExists(int id)
		{
			return _context.Release.Any(e => e.ID == id);
		}
	}
}
