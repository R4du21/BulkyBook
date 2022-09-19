using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.Utility;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if(_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }
            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                //if roles are not created, then we create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@backend.com",
                    Email = "admin@backend.com",
                    Name = "New AdminUser",
                    PhoneNumber = "07668666877",
                    StreetAdress = "test 123 Ave",
                    State = "IL",
                    PostalCode = "1234",
                    City = "Chicago"
                }, "Test2001.").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@backend.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
