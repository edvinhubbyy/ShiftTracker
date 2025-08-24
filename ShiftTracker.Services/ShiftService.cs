using Microsoft.EntityFrameworkCore;
using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using ShiftTracker.Services.Core.Contracts;

public class ShiftService : IShiftService
{
    private readonly ShiftContext _context;

    public ShiftService(ShiftContext context)
    {
        _context = context;
    }

    public async Task<List<Shift>> GetShiftsForEmployeeAsync(Guid employeeId)
    {
        return await _context.Shifts
            .Include(s => s.Breaks)
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.CheckInTime)
            .ToListAsync();
    }

    public async Task<Shift> StartShiftAsync(Guid employeeId)
    {
        var activeShift = await _context.Shifts
            .Where(s => s.EmployeeId == employeeId && s.CheckOutTime == null)
            .FirstOrDefaultAsync();

        if (activeShift != null)
            throw new InvalidOperationException("There is already an active shift.");

        var shift = new Shift
        {
            EmployeeId = employeeId,
            CheckInTime = DateTime.Now
        };

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();

        // Record audit log
        _context.AuditLogs.Add(new AuditLog
        {
            EmployeeId = employeeId,
            EventType = "Shift Started",
            Timestamp = DateTime.Now,
            ShiftId = shift.Id
        });
        await _context.SaveChangesAsync();

        return shift;
    }

    public async Task EndShiftAsync(int shiftId)
    {
        var shift = await _context.Shifts.FindAsync(shiftId);
        if (shift == null)
            throw new ArgumentException("Shift not found", nameof(shiftId));

        shift.CheckOutTime = DateTime.Now;
        await _context.SaveChangesAsync();

        _context.AuditLogs.Add(new AuditLog
        {
            EmployeeId = shift.EmployeeId,
            EventType = "Shift Ended",
            Timestamp = DateTime.Now,
            ShiftId = shift.Id
        });
        await _context.SaveChangesAsync();
    }

    public async Task<Break> StartBreakAsync(int shiftId)
    {
        var shift = await _context.Shifts.FindAsync(shiftId);
        if (shift == null)
            throw new ArgumentException("Shift not found", nameof(shiftId));

        var activeBreak = await _context.Breaks
            .Where(b => b.ShiftId == shiftId && b.EndTime == null)
            .FirstOrDefaultAsync();

        if (activeBreak != null)
            throw new InvalidOperationException("There is already an active break.");

        var br = new Break
        {
            ShiftId = shiftId,
            StartTime = DateTime.Now
        };

        _context.Breaks.Add(br);
        await _context.SaveChangesAsync();

        _context.AuditLogs.Add(new AuditLog
        {
            EmployeeId = shift.EmployeeId,
            ShiftId = shift.Id,
            EventType = "Break Started",
            Timestamp = DateTime.Now
        });
        await _context.SaveChangesAsync();

        return br;
    }

    public async Task EndBreakAsync(int breakId)
    {
        var br = await _context.Breaks.FindAsync(breakId);
        if (br == null)
            throw new ArgumentException("Break not found", nameof(breakId));

        br.EndTime = DateTime.Now;
        await _context.SaveChangesAsync();

        var shift = await _context.Shifts.FindAsync(br.ShiftId);
        if (shift != null)
        {
            _context.AuditLogs.Add(new AuditLog
            {
                EmployeeId = shift.EmployeeId,
                ShiftId = shift.Id,
                EventType = "Break Ended",
                Timestamp = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
    }
}