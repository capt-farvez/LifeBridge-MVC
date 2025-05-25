using Microsoft.AspNetCore.Mvc;
using LifeBridge.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeBridge.Controllers
{
    public class BloodDonationController : Controller
    {
        private readonly AppDbContext _context;

        public BloodDonationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Show blood donation request form
        [HttpGet("donation/blood/request")]
        public IActionResult RequestBloodDonation()
        {
            return View();
        }

        // POST: Submit blood donation request
        [HttpPost("donation/blood/request")]
        public async Task<IActionResult> RequestBloodDonation(RequestBloodDonation request)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return RedirectToAction("Login", "User");
            }

            request.Id = Guid.NewGuid();
            request.UserId = userId;
            request.RequestDate = DateTime.UtcNow;

            _context.BloodDonationRequests.Add(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your blood donation request was submitted successfully.";
            return RedirectToAction("MyRequests");
        }

        // GET: View current user's blood donation requests
        [HttpGet("donation/blood/my-requests")]
        public async Task<IActionResult> MyRequests()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return RedirectToAction("Login", "User");
            }

            var requests = await _context.BloodDonationRequests
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(requests);
        }

        // GET: Edit request form
        [HttpGet("donation/blood/edit/{id}")]
        public async Task<IActionResult> EditRequest(Guid id)
        {
            var request = await _context.BloodDonationRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (request.UserId.ToString() != userIdStr)
            {
                return Forbid();
            }

            return View(request);
        }

        // POST: Submit edit
        [HttpPost("donation/blood/edit/{id}")]
        public async Task<IActionResult> EditRequest(Guid id, RequestBloodDonation updatedRequest)
        {
            var existingRequest = await _context.BloodDonationRequests.FindAsync(id);
            if (existingRequest == null)
            {
                return NotFound();
            }

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (existingRequest.UserId.ToString() != userIdStr)
            {
                return Forbid();
            }

            existingRequest.Bloodgroup = updatedRequest.Bloodgroup;
            existingRequest.Location = updatedRequest.Location;
            existingRequest.Perpose = updatedRequest.Perpose;
            existingRequest.Phone = updatedRequest.Phone;

            _context.BloodDonationRequests.Update(existingRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request updated successfully.";
            return RedirectToAction("MyRequests");
        }

        // POST: Delete request
        [HttpPost("donation/blood/delete/{id}")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            var request = await _context.BloodDonationRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (request.UserId.ToString() != userIdStr)
            {
                return Forbid();
            }

            _context.BloodDonationRequests.Remove(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request deleted successfully.";
            return RedirectToAction("MyRequests");
        }
    }
}
