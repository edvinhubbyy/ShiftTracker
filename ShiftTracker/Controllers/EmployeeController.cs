using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using ShiftTracker.Services.Core.Contracts;
using ShiftTracker.ViewModels.Employees;
using System;
using System.Threading.Tasks;
using iText.Layout;

namespace ShiftTracker.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IShiftService _shiftService;
        private readonly IEmployeeService _employeeService;
        private readonly ShiftContext _context;

        public EmployeeController(IShiftService shiftService, IEmployeeService employeeService, ShiftContext context)
        {
            _shiftService = shiftService;
            _employeeService = employeeService;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("StartStopShift"); // or your actual main action
        }

        [HttpPost]
        public async Task<IActionResult> DownloadEmployeePdf(Guid employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return NotFound();

            // Generate QR code
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(employee.CardId, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20);

            // Create PDF in memory
            using var pdfStream = new MemoryStream();
            using var writer = new PdfWriter(pdfStream);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph($"Employee: {employee.Name}").SetFontSize(16));
            doc.Add(new Paragraph($"Card ID: {employee.CardId}").SetFontSize(14));

            var qrImage = new Image(ImageDataFactory.Create(qrBytes));
            qrImage.SetWidth(200).SetHeight(200);
            doc.Add(qrImage);

            doc.Close();

            return File(pdfStream.ToArray(), "application/pdf", $"{employee.CardId}.pdf");
        }


        public async Task<IActionResult> StartStopShift()
        {
            // Get EmployeeId from session
            var employeeIdStr = HttpContext.Session.GetString("EmployeeId");

            if (string.IsNullOrEmpty(employeeIdStr))
            {
                // No session found, redirect to login
                return RedirectToAction("Login", "Auth");
            }

            if (!Guid.TryParse(employeeIdStr, out var employeeId))
            {
                // Invalid GUID in session, clear session and redirect
                HttpContext.Session.Remove("EmployeeId");
                return RedirectToAction("Login", "Auth");
            }

            // Get employee and shifts
            var employee = await _employeeService.GetByIdAsync(employeeId);
            if (employee == null)
            {
                HttpContext.Session.Remove("EmployeeId");
                return RedirectToAction("Login", "Auth");
            }

            var shifts = await _shiftService.GetShiftsForEmployeeAsync(employeeId);

            var model = new EmployeeShiftViewModel
            {
                EmployeeId = employee.Id, 
                EmployeeName = employee.Name,
                CurrentShift = shifts.FirstOrDefault(s => s.CheckOutTime == null),
                ShiftHistory = shifts
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> StartShift()
        {
            var employeeIdStr = HttpContext.Session.GetString("EmployeeId");

            var employeeId = Guid.Parse(employeeIdStr);

            await _shiftService.StartShiftAsync(employeeId);

            TempData["Message"] = "Shift started!";
            return RedirectToAction("StartStopShift");
        }

        [HttpPost]
        public async Task<IActionResult> EndShift()
        {
            var employeeIdStr = HttpContext.Session.GetString("EmployeeId");
            if (string.IsNullOrEmpty(employeeIdStr))
                return RedirectToAction("Login", "Auth");

            var employeeId = Guid.Parse(employeeIdStr);

            var shifts = await _shiftService.GetShiftsForEmployeeAsync(employeeId);
            var activeShift = shifts.FirstOrDefault(s => s.CheckOutTime == null);

            if (activeShift == null)
            {
                TempData["Error"] = "No active shift found.";
                return RedirectToAction("StartStopShift");
            }

            await _shiftService.EndShiftAsync(activeShift.Id);

            TempData["Message"] = "Shift ended successfully!";
            return RedirectToAction("StartStopShift");
        }

        [HttpPost]
        public async Task<IActionResult> StartBreak(int shiftId)
        {
            await _shiftService.StartBreakAsync(shiftId);
            return RedirectToAction("StartStopShift");
        }

        [HttpPost]
        public async Task<IActionResult> EndBreak(int breakId)
        {
            await _shiftService.EndBreakAsync(breakId);
            return RedirectToAction("StartStopShift");
        }

    }
}