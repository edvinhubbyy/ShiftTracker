using Microsoft.EntityFrameworkCore;
using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using ShiftTracker.Services.Core.Contracts;

public class EmployeeService : IEmployeeService
{
    private readonly ShiftContext _context;

    public EmployeeService(ShiftContext context)
    {
        _context = context;
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

    public async Task<Employees?> GetByCardIdAsync(string cardId)
    {
        return await _context.Employees
            .Include(e => e.Position)
            .Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.CardId == cardId);
    }

    

    public async Task<bool> AuthenticateCardAsync(string cardId, string? pin = null)
    {
        var employee = await GetByCardIdAsync(cardId);
        if (employee == null) return false;

        if (!string.IsNullOrEmpty(pin) && employee.Pin != pin)
            return false;

        return true;
    }
}