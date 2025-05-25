using LifeBridge.Models;
using LifeBridge.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace LifeBridge.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("admin/dashboard")]
        public async Task<IActionResult> Dashboard(){
            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = _context.Users.Count(),
                TotalBloodRequests = _context.BloodDonationRequests.Count(),
                TotalOrganRequests = _context.OrganDonationRequests.Count()
            };

            return View(viewModel); 
        }
        //  ------------------- Contact / Message Section -------------------//
        //  It retrieves all messages from the database (Message Model) and returns them to the view
        [HttpGet("admin/all-messages")]
        public IActionResult Messages()
        {
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        //  To view the message Details
        [HttpGet("admin/messages/details/{id}")]
        public IActionResult MessageDetails(int id)
        {   
            var message = _context.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message); 
        }

         //  The Delete method handles the deletion of a message from the database. It retrieves the message by ID, removes it, and saves the changes.
        [HttpPost("admin/messages/delete/{id}")]
        public IActionResult DeleteMessage(int id)
        {
            var message = _context.Messages.Find(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
            return RedirectToAction("Messages");
        }

        //  ------------------- User Management Section -------------------//
        // To Get All Users
        [HttpGet]
        [Route("admin/users/all")]
        public IActionResult AllUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        //  Create new user view page
        [HttpGet("admin/create-user")]
        public IActionResult CreateUser(){
            return View();
        }

        //  Create new user post page
        [HttpPost("admin/create-user")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            // Check if the email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(user);
            }

            // Hash the password before saving
            user.Password = HashPassword(user.Password);
            user.Id = Guid.NewGuid(); // Generate a new GUID for the user ID

            // Save the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful. You can now log in.";
            return RedirectToAction("Dashboard");

        }

        // Simple SHA256 hashing
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}