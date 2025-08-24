using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using ShiftTracker.Services.Core.Contracts;
using ShiftTracker.ViewModels.Employees;

namespace ShiftTracker.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        private readonly ShiftContext _context;

        public ManagerController(IManagerService managerService, ShiftContext context)
        {
            _managerService = managerService;
            _context = context;
        }

        // GET: /Manager
        public async Task<IActionResult> Index()
        {
            var employees = await _managerService.GetAllAsync();
            var model = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                Name = e.Name,
                CardId = e.CardId,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                RoleName = e.Role?.Name ?? "Unknown",
                PositionName = e.Position?.Name ?? "Unknown"
            }).ToList();

            return View("Index", model);
        }

        public async Task<IActionResult> Dashboard()
        {
            // Get all employees with Role and Position info
            var employees = await _managerService.GetAllAsync();

            // Map to view model
            var model = employees.Select(e => new EmployeeViewModel
            {
                Id = e.Id,
                Name = e.Name,
                CardId = e.CardId,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                RoleName = e.Role?.Name ?? "Unknown",
                PositionName = e.Position?.Name ?? "Unknown"
            }).ToList();

            return View("Dashboard", model);
        }


        // GET: /Manager/Create
        public async Task<IActionResult> Create()
        {
            var model = new EmployeeCreateViewModel
            {
                Positions = await _context.Positions.ToListAsync(),
                Roles = await _context.Roles.ToListAsync()
            };
            return View("Create", model);
        }

        // POST: /Manager/Create
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Positions = await _context.Positions.ToListAsync();
                model.Roles = await _context.Roles.ToListAsync();
                return View("Create", model);
            }

            var employee = new Employees
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                CardId = model.CardId,
                Pin = model.Pin,
                PositionId = model.PositionId,
                RoleId = model.RoleId,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            try
            {
                await _managerService.CreateAsync(employee);
                TempData["Success"] = "Employee created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) // handled in service
            {
                ModelState.AddModelError("CardId", ex.Message);
                model.Positions = await _context.Positions.ToListAsync();
                model.Roles = await _context.Roles.ToListAsync();
                return View("Create", model);
            }
        }

        // GET: /Manager/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var employee = await _managerService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var model = new EmployeeCreateViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                CardId = employee.CardId,
                Pin = employee.Pin,
                PositionId = employee.PositionId,
                RoleId = employee.RoleId,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Positions = await _context.Positions.ToListAsync(),
                Roles = await _context.Roles.ToListAsync()
            };

            return View("Edit", model);
        }

        // POST: /Manager/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EmployeeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Positions = await _context.Positions.ToListAsync();
                model.Roles = await _context.Roles.ToListAsync();
                return View("Edit", model);
            }

            var employee = await _managerService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            employee.Name = model.Name;
            employee.CardId = model.CardId;
            employee.Pin = model.Pin;
            employee.PositionId = model.PositionId;
            employee.RoleId = model.RoleId;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Email = model.Email;

            try
            {
                await _managerService.UpdateAsync(employee);
                TempData["Success"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) // duplicate CardId or other handled error
            {
                ModelState.AddModelError("CardId", ex.Message);
                model.Positions = await _context.Positions.ToListAsync();
                model.Roles = await _context.Roles.ToListAsync();
                return View("Edit", model);
            }
        }


        // POST: /Manager/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _managerService.DeleteAsync(id);
            TempData["Success"] = "Employee deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}