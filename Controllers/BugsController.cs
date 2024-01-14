using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BugTrackerMVC.Models;
using Microsoft.EntityFrameworkCore;
using BugTrackerMVC.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.ComponentModel.Design;

namespace BugTrackerMVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BugsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BugsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Bugs()
        {
            var bugs = await _context.Bugs.ToListAsync();
            return View(bugs);
        }

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var bug = _context.Bugs.Include(e => e.Comments).FirstOrDefault(e => e.Id == id);
            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }

        [AllowAnonymous]
        public IActionResult CommentDetails(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var comment = _context.Comments.Include(e => e.SubComments).FirstOrDefault(e => e.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(string AuthorId, [Bind("AuthorId,Title,Description,ApplicationUserId")] Bug bug)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Bugs));
            }
            return View(bug);
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddComment(int bugId, [Bind("AuthorId,Title,Content")] Comment commentModel)
        {
            if (ModelState.IsValid)
            {
                var bug = _context.Bugs.Include(e => e.Comments).FirstOrDefault(e => e.Id == bugId);

                if (bug != null)
                {
                    var newComment = new Comment
                    {
                        AuthorId = commentModel.AuthorId,
                        Title = commentModel.Title,
                        Content = commentModel.Content,
                        BugId = bugId,
                    };

                    bug.Comments.Add(newComment);
                    _context.SaveChanges();
                }

                return RedirectToAction("Details", new { id = bugId });
            }
            return RedirectToAction("Details", new { id = bugId });
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddSubComment(int commentId, [Bind("AuthorId,Title,Content")] SubComment commentModel)
        {
            if (ModelState.IsValid)
            {
                var comment = _context.Comments.Include(e => e.SubComments).FirstOrDefault(e => e.Id == commentId);

                if (commentId != null)
                {
                    var newComment = new SubComment
                    {
                        AuthorId = commentModel.AuthorId,
                        Title = commentModel.Title,
                        Content = commentModel.Content,
                        CommentId = commentId,
                    };

                    comment.SubComments.Add(newComment);
                    _context.SaveChanges();
                }

                return RedirectToAction("CommentDetails", new { id = commentId });
            }
            return RedirectToAction("CommentDetails", new { id = commentId });
        }

        [AllowAnonymous]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = _context.Bugs.FirstOrDefault(e => e.Id == id);

            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,Title,Status,Description,ApplicationUserId")] Bug bug)
        {
            if (id != bug.Id)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
         
            if ((currentUser == null || bug.AuthorId != currentUser.Id) && !currentUserRole.Contains("Administrator"))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugExists(bug.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Bugs));
            }
            return View(bug);
        }

        [AllowAnonymous]
        public IActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = _context.Comments.FirstOrDefault(e => e.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }
        [AllowAnonymous]
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCommentConfirmed(int id)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var comment = await _context.Comments.FindAsync(id);
            var redirectto = comment.BugId;

            if ((currentUser == null || comment.AuthorId != currentUser.Id) && !currentUserRole.Contains("Administrator"))
            {
                return Forbid();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = redirectto });
        }
        [AllowAnonymous]
        public IActionResult DeleteSubComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcomment = _context.SubComments.FirstOrDefault(e => e.Id == id);

            if (subcomment == null)
            {
                return NotFound();
            }

            return View(subcomment);
        }
        [AllowAnonymous]
        [HttpPost, ActionName("DeleteSubComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubCommentConfirmed(int id)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var subcomment = await _context.SubComments.FindAsync(id);
            var redirectto = subcomment.CommentId;

            if ((currentUser == null || subcomment.AuthorId != currentUser.Id) && !currentUserRole.Contains("Administrator"))
            {
                return Forbid();
            }

            _context.SubComments.Remove(subcomment);
            await _context.SaveChangesAsync();
            return RedirectToAction("CommentDetails", new { id = redirectto });
        }

        [AllowAnonymous]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = _context.Bugs.FirstOrDefault(e => e.Id == id);

            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }


 
        [AllowAnonymous]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var bug = await _context.Bugs.FindAsync(id);

            if ((currentUser == null || bug.AuthorId != currentUser.Id) && !currentUserRole.Contains("Administrator"))
            {
                return Forbid();
            }
            
            _context.Bugs.Remove(bug);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Bugs));
        }

        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.Id == id);
        }
    }
}
