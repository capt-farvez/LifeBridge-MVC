using LifeBridge.Models;
using LifeBridge.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeBridge.Controllers
{
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
                TotalUsers = _context.Users.Count()
            //     TotalBloodRequests = _context.BloodRequests.Count(),
            //     TotalBloodAvailabilities = _context.BloodDonationAvailabilities.Count(),
            //     TotalOrganRequests = _context.OrganRequests.Count(),
            //     TotalOrganAvailabilities = _context.OrganDonationAvailabilities.Count()
            };

            return View(viewModel);
            

        }

    }
}