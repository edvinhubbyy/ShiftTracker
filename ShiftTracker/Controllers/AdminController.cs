using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTracker.Data.Models;
using ShiftTracker.Services.Core.Contracts;

namespace ShiftTracker.Controllers
{
    [Authorize(Roles = "Admin")] // only Admin
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public AdminController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Admin Dashboard
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            return View(employees);
        }
    }
}