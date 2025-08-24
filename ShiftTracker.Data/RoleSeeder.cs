using ShiftTracker.Data;
using ShiftTracker.Data.Models;
using System.Linq;

namespace ShiftTracker.Seeders
{
    public static class RoleSeeder
    {
        public static void SeedAll(ShiftContext context)
        {
            // 1️⃣ Seed Roles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Employee" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Admin" }
                );
                context.SaveChanges();
            }

            // 2️⃣ Seed Positions
            if (!context.Positions.Any())
            {
                context.Positions.AddRange(
                    new Position { Name = "Cashier" },
                    new Position { Name = "Cook" },
                    new Position { Name = "Cleaner" }
                );
                context.SaveChanges();
            }

            // 3️⃣ Seed default Admin
            if (!context.Employees.Any(e => e.Role.Name == "Admin"))
            {
                var adminRole = context.Roles.First(r => r.Name == "Admin");
                var position = context.Positions.First(); // pick any position
                context.Employees.Add(new Employees
                {
                    Name = "Default Admin",
                    CardId = "ADMIN123",
                    Pin = "0000",
                    Email = "admin@example.com",
                    PhoneNumber = "0000000000",
                    RoleId = adminRole.Id,
                    PositionId = position.Id
                });
            }

            // 4️⃣ Seed default Manager
            if (!context.Employees.Any(e => e.Role.Name == "Manager"))
            {
                var managerRole = context.Roles.First(r => r.Name == "Manager");
                var position = context.Positions.First();
                context.Employees.Add(new Employees
                {
                    Name = "Default Manager",
                    CardId = "MANAGER123",
                    Pin = "1111",
                    Email = "manager@example.com",
                    PhoneNumber = "1111111111",
                    RoleId = managerRole.Id,
                    PositionId = position.Id
                });
            }

            context.SaveChanges();
        }
    }
}