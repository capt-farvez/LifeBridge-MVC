using LifeBridge.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;

namespace LifeBridge.Controllers
{
    public class ContactController : Controller
    {

        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        //  To display the contact form, we use the Contact method.
        [HttpGet]
        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        //  The Contact method handles the form submission. It checks if the model state is valid, saves the message to the database, and redirects to a thank-you page.
        [HttpPost]
        [Route("contact")]
        public IActionResult Contact(Message message)
        {
            if (ModelState.IsValid)
            {
                // Save the message to the database
                message.SentAt = DateTime.Now;
                _context.Messages.Add(message);
                _context.SaveChanges();
                return RedirectToAction("ThankYou");
            }
            return View(message);
        }

        //  The ThankYou method displays a thank-you page after the form submission.
        [HttpGet]
        [Route("contact/thank-you")]
        public IActionResult ThankYou()
        {
            return View();
        }

    }
}