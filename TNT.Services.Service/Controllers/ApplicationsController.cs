using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models;
using TNT.Services.Service.Models.Entities;

namespace TNT.Update.Service.Controllers
{
	[Authorize]
	public class ApplicationsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ApplicationsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Applications
		public IActionResult Index()
		{
			var applications = _context.Application.ToList();
			var releases = _context.Release.ToList();

			var applicationPluses = (from a in applications
															 join r in releases on a.ID equals r.ApplicationID into join1
															 from r1 in join1.DefaultIfEmpty()
															 select new ApplicationPlus() { ID = a.ID, Name = a.Name, Version = r1 == null ? "" : r1.Version });

			return View(applicationPluses);
		}

		// GET: Applications/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var application = await _context.Application
					.FirstOrDefaultAsync(m => m.ID == id);
			if (application == null)
			{
				return NotFound();
			}

			return View(application);
		}

		// GET: Applications/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Applications/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Name")] Application application)
		{
			if (ModelState.IsValid)
			{
				_context.Add(application);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(application);
		}

		// GET: Applications/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var application = await _context.Application.FindAsync(id);
			if (application == null)
			{
				return NotFound();
			}
			return View(application);
		}

		// POST: Applications/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Application application)
		{
			if (id != application.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(application);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ApplicationExists(application.ID))
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
			return View(application);
		}

		// GET: Applications/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var application = await _context.Application
					.FirstOrDefaultAsync(m => m.ID == id);
			if (application == null)
			{
				return NotFound();
			}

			return View(application);
		}

		// POST: Applications/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var application = await _context.Application.FindAsync(id);
			_context.Application.Remove(application);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ApplicationExists(int id)
		{
			return _context.Application.Any(e => e.ID == id);
		}
	}
}
