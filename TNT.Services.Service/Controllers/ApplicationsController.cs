﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TNT.Commons;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace TNT.Update.Service.Controllers;

[Authorize]
public class ApplicationsController : Controller
{
  private readonly ApplicationDbContext _context;

  public ApplicationsController(ApplicationDbContext context)
  {
    _context = context;
  }

  // GET: Applications
  public async Task<IActionResult> Index()
  {
    return View(await _context.Application.ToListAsync());
  }

  // GET: Applications/Details/5
  public async Task<IActionResult> Details(Guid? id)
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
  public async Task<IActionResult> Create([Bind("ID,Name,Secret")] Application application)
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
  public async Task<IActionResult> Edit(Guid? id)
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
  public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name,Secret")] Application application)
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
  public async Task<IActionResult> Delete(Guid? id)
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
  public async Task<IActionResult> DeleteConfirmed(Guid id)
  {
    Application? application = await _context.Application.FindAsync(id);
    application?.also(async it =>
     {
       _context.Application.Remove(it);
       await _context.SaveChangesAsync();
     });
    return RedirectToAction(nameof(Index));
  }

  private bool ApplicationExists(Guid id)
  {
    return _context.Application.Any(e => e.ID == id);
  }
}
