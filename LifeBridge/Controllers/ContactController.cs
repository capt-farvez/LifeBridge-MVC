using LifeBridge.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;

namespace LifeBridge.Controllers
{   
    public class ContactController : Controller
    {
        
        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("contact/messages")]
        public IActionResult Messages()
        {
            // This action would typically retrieve messages from a database
            return View();
        }

        [HttpGet]
        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("contact")]
        public IActionResult Contact(Message message)
        {
            if (ModelState.IsValid)
            {
                // Save the message to the database or send an email
                // For now, we'll just log it
                Console.WriteLine($"Message from {message.SenderName} ({message.Email}): {message.Content}");
                // Log the message using the logger
                _logger.LogInformation($"Message from {message.SenderName} ({message.Email}): {message.Content}");
                return RedirectToAction("ThankYou");
            }
            return View(message);
        }

        [Route("contact/thank-you")]
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
