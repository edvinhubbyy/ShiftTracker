using Microsoft.AspNetCore.Mvc;
using ShiftTracker.Services.Core.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ShiftTracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public AuthController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string cardId)
        {
            if (string.IsNullOrWhiteSpace(cardId))
                return View("Login");

            var employee = await _employeeService.GetByCardIdAsync(cardId);
            if (employee == null)
                return View("Login");

            // Build claims for authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.Name ?? employee.CardId),
                new Claim(ClaimTypes.Role, employee.Role?.Name ?? "Employee")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Sign the user in (creates authentication cookie)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Set session for employee
            HttpContext.Session.SetString("EmployeeId", employee.Id.ToString());

            // Redirect based on role
            switch (employee.Role?.Name)
            {
                case "Employee":
                    return RedirectToAction("StartStopShift", "Employee");
                case "Manager":
                case "Admin":
                    return RedirectToAction("Dashboard", "Manager");
                default:
                    return View("Login");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}