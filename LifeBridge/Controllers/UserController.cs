using LifeBridge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


namespace LifeBridge.Controllers
{   
    
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        //  To Get Registration Page
        [HttpGet]
        [Route("users/register")]
        public IActionResult Register()
        {
            return View();
        }

        // To Post Registration Data
        [HttpPost]
        [Route("users/register")]
        public async Task<IActionResult> Register(User user)
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
            user.Role = Role.User;

            // Save the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful. You can now log in.";
            return RedirectToAction("Login");
        }

        // To Get Login Page
        [HttpGet]
        [Route("users/login")]
        public IActionResult Login()
        {
            return View();
        }
        // To Post Login Data
        [HttpPost]
        [Route("users/login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null || !VerifyPassword(Password, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                return View();
            }
            // Set user session or authentication cookie here
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.Name ?? "User");
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            // Set user authentication cookie here
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? "User"),
                new Claim(ClaimTypes.Role, user.Role.ToString()) // For [Authorize(Roles = "Admin" or "User")]
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            if (user.Role == Role.Admin)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return RedirectToRoute(new { controller = "User", action = "Profile", id = user.Id });
        }

        // To Get Logout Page
        [HttpGet]
        [Route("users/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync("MyCookieAuth");
            return View();
        }

        // To get Password Change Page
        [HttpGet]
        [Route("users/{id}/change-password")]
        public IActionResult ChangePassword()
        {
            var id = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login");
            }
            var userId = Guid.Parse(id);
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            Console.WriteLine("In ChangePassword Get");
            return View(user);
        }

        // To Post Password Change Data
        [HttpPost]
        [Route("users/{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("", "Current and new passwords are required.");
                return View();
            }
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "New password and confirmation do not match.");
                return View();
            }
            var id = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login");
            }
            var userId = Guid.Parse(id);
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!VerifyPassword(currentPassword, user.Password))
            {
                ModelState.AddModelError("", "Current password is incorrect.");
                return View(user);
            }

            // Hash the new password before saving
            user.Password = HashPassword(newPassword);
            _context.SaveChanges();
            Console.WriteLine("In ChangePassword Post");

            TempData["Success"] = "Your Password changed successfully.";
            return RedirectToRoute(new { controller = "User", action = "Profile", id = user.Id });
        }



        // To Get User Profile Page
        // [Authorize]
        [HttpGet]
        [Route("users/{id}/profile")]
        public IActionResult Profile()
        {
            var id = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login");
            }
            var userId = Guid.Parse(id);
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // User profile update
        [HttpGet]
        [Route("users/{id}/update")]
        public IActionResult UpdateProfile()
        {
            var id = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login");
            }
            var userId = Guid.Parse(id);
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View("UpdateProfile", user);
        }

        [HttpPost]
        [Route("users/{id}/update")]
        public IActionResult UpdateProfile(User user)
        {   
            var id = HttpContext.Session.GetString("UserId");
            var userId = Guid.Parse(id);
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login");
            }           

            var existingUser = _context.Users.Find(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            // Update the user properties
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.BloodGroup = user.BloodGroup;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToRoute(new { controller = "User", action = "Profile", id = user.Id });


        }

        // To Get User Details
        [HttpGet]
        [Route("users/{id}")]
        public IActionResult Details(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        // Simple SHA256 hashing
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            using var sha256 = SHA256.Create();  // Create a new instance of SHA256
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));  // Hash the input password
            var inputHash = Convert.ToBase64String(hash); // Convert the hash to a base64 string
            return inputHash == storedHash; // Compare the hashes
        }
    }
}