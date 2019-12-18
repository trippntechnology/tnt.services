using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TNT.Services.Models.Exceptions;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models;
using TNT.Services.Service.Models.Entities;


namespace TNT.Update.Service.Controllers
{
	public class ReleasesController : Controller
	{
		const string TEMP_FILE_PATH = "wwwroot/uploads";
		private readonly ApplicationDbContext _context;

		public ReleasesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Releases
		public IActionResult Index()
		{
			var applications = _context.Application.ToList();
			var releases = _context.Release.ToList();

			var releasePluses = (from a in applications
													 join r in releases on a.ID equals r.ApplicationID
													 select new ReleasePlus()
													 {
														 ApplicationID = a.ID,
														 ID = r.ID,
														 ApplicationName = a.Name,
														 Version = (r == null ? "" : r.Version),
														 Date = (r == null ? DateTime.Now : r.Date),
														 FileName = (r == null ? "" : r.FileName)
													 });

			return View(releasePluses.OrderBy(r => r.Date));
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
			var applications = _context.Application.OrderBy(a => a.Name).ToList();
			ViewBag.Applications = new SelectList(applications, "ID", "Name");
			return View();
		}

		// POST: Releases/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,ApplicationID,Package")] Release release, IFormFile upload)
		{
			if (ModelState.IsValid)
			{
				var application = _context.Application.Find(release.ApplicationID);
				if (application == null) throw new InvalidApplicationIdException();

				using (var reader = new BinaryReader(upload.OpenReadStream()))
				{
					release.Package = reader.ReadBytes((int)upload.Length);
				}

				release.Version = GetVersion(release.Package);// fileVersion.Version;
				release.Date = DateTime.Now;
				release.FileName = upload.FileName;

				_context.Add(release);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(release);
		}

		public string GetVersion(byte[] package)
		{
			string version = String.Empty;
			string fileName = String.Empty;

			try
			{
				if (!Directory.Exists(TEMP_FILE_PATH))
				{
					Directory.CreateDirectory(TEMP_FILE_PATH);
				}

				fileName = Path.Combine(TEMP_FILE_PATH, Guid.NewGuid().ToString());
				System.IO.File.WriteAllBytes(fileName, package);
				version = FileVersionInfo.GetVersionInfo(fileName)?.FileVersion;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			finally
			{
				if (!String.IsNullOrEmpty(fileName)) System.IO.File.Delete(fileName);
			}

			return version;
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
			var application = await _context.Application.FirstOrDefaultAsync(a => a.ID == release.ApplicationID);

			release.Package = null;

			var releasePlus = new ReleasePlus()
			{
				ApplicationID = application.ID,
				ApplicationName = application.Name,
				Date = release.Date,
				ID = release.ID,
				FileName = release.FileName,
				Version = release.Version
			};

			return View(releasePlus);
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
