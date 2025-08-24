using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using ShiftTracker.Services.Core.Contracts;

public class ManagerService : IManagerService
{
    private readonly ShiftContext _context;

    public ManagerService(ShiftContext context)
    {
        _context = context;
    }

    public async Task RecordActionAsync(Guid managerId, string action, Guid? employeeId = null)
    {
        var managerAction = new ManagerAction
        {
            ManagerId = managerId,
            EmployeeId = employeeId,
            Action = action,
            Timestamp = DateTime.Now
        };

        _context.ManagerActions.Add(managerAction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Employees>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.Role)
            .ToListAsync();
    }

    public async Task<Employees?> GetByIdAsync(Guid employeeId)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
    }

    public async Task CreateAsync(Employees employee)
    {
        try
        {
            // Add employee to DB
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Generate QR code bytes
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(employee.CardId, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20); // 20 = pixels per module

            // Create PDF
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

            // Save PDF to disk
            string pdfPath = Path.Combine("wwwroot", "qrcodes", $"{employee.CardId}.pdf");
            Directory.CreateDirectory(Path.GetDirectoryName(pdfPath)!);
            await File.WriteAllBytesAsync(pdfPath, pdfStream.ToArray());
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Employees_CardId"))
                throw new InvalidOperationException("Card ID already exists. Please choose another.");

            throw;
        }
    }


    public async Task UpdateAsync(Employees employee)
    {
        try
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Employees_CardId"))
                throw new InvalidOperationException("Card ID already exists. Please choose another.");

            throw;
        }
    }



    public async Task DeleteAsync(Guid employeeId)
    {
        var employee = await GetByIdAsync(employeeId);
        if (employee != null)
        {
            employee.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }


    public async Task AdjustShiftAsync(Guid employeeId, DateTime? checkInTime, DateTime? checkOutTime)
    {
        var shift = await _context.Shifts
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.CheckInTime)
            .FirstOrDefaultAsync();

        if (shift == null)
            throw new ArgumentException("Shift not found");

        if (checkInTime.HasValue)
            shift.CheckInTime = checkInTime.Value;

        if (checkOutTime.HasValue)
            shift.CheckOutTime = checkOutTime.Value;

        await _context.SaveChangesAsync();

        await RecordActionAsync(managerId: Guid.Empty, // optional managerId
                                action: $"Adjusted shift for employee {employeeId}",
                                employeeId: employeeId);
    }

    public async Task ResetPinAsync(Guid employeeId, string newPin)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
            throw new ArgumentException("Employee not found");

        employee.Pin = newPin;
        await _context.SaveChangesAsync();

        await RecordActionAsync(managerId: Guid.Empty, action: $"Reset PIN for employee {employeeId}", employeeId);
    }
}