using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog_App.Data;
using Blog_App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Blog_App.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<NotesController> _logger;

        public NotesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<NotesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Note.Where( x => x.IsPublic == true ) .ToListAsync());
        }

        // GET: Notes/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Notes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Note.Where( 
                x => x.NoteText.Contains(SearchPhrase) && x.IsPublic == true ).ToListAsync());
        }

        // GET: Notes/ShowSearchForm
        public async Task<IActionResult> ShowPublicNotes()
        {
            return View("ShowPublicNotes", await _context.Note.Where( 
                x => (x.IsPublic == true) && (x.NoteAuthor == User.Identity.Name)).ToListAsync());
        }

        // GET: Notes/ShowSearchForm
        public async Task<IActionResult> ShowPrivateNotes()
        {
            return View("ShowPrivateNotes", await _context.Note.Where(
                x => (x.IsPublic == false) && (x.NoteAuthor == User.Identity.Name)).ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoteTitle,NoteText,IsPublic")] Note note)
        {
            // set author to current user (remove from validation as not included in form)
            note.NoteAuthor = User.Identity.Name;
            ModelState.Remove(nameof(Note.NoteAuthor));

            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogInformation("CREATING ERRORS: ");
                foreach (var val in ModelState)
                {
                    foreach (var error in val.Value.Errors)
                    {
                        _logger.LogInformation(error.ErrorMessage);
                    }
                }
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoteTitle,NoteText")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            // set author to current user (remove from validation as not included in form)
            note.NoteAuthor = User.Identity.Name;
            ModelState.Remove(nameof(Note.NoteAuthor));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            else
            {
                _logger.LogInformation("EDITTING ERRORS: ");
                foreach (var val in ModelState)
                {
                    foreach (var error in val.Value.Errors)
                    {
                        _logger.LogInformation(error.ErrorMessage);
                    }
                }
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            if (note != null)
            {
                _context.Note.Remove(note);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}
