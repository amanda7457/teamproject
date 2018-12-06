using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using System.Collections.Generic;
namespace Group14_BevoBooks.Seeding
{
    public class SeedManagers
    {

        public static async Task SeedAllManagersAsync(IServiceProvider serviceProvider)
        {
            AppDbContext _db = serviceProvider.GetRequiredService<AppDbContext>();
            UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            //Int32 intCustomersAdded = 0;
            //String CustomerName = "Begin"; //helps to keep track of error on books
            List<AppUser> Managers = new List<AppUser>();

            try
            {
                AppUser m1 = new AppUser();
                m1.PasswordHash = "dewey4";
                m1.LastName = "Baker";
                m1.FirstName = "Christopher";
                m1.Address = "1245 Lake Libris Dr.";
                m1.City = "Cedar Park";
                m1.State = "TX";
                m1.Zipcode = "78613";
                m1.Email = "c.baker@bevosbooks.com";
                m1.UserName = "c.baker@bevosbooks.com";
                m1.PhoneNumber = "3395325649";
                await _userManager.AddToRoleAsync(m1, "Manager");

                await _userManager.CreateAsync(m1, "dewey4");
                Managers.Add(m1);

                AppUser m2 = new AppUser();
                m2.PasswordHash = "arched";
                m2.LastName = "Rice";
                m2.FirstName = "Eryn";
                m2.Address = "3405 Rio Grande";
                m2.City = "Austin";
                m2.State = "TX";
                m2.Zipcode = "78746";
                m2.Email = "e.rice@bevosbooks.com";
                m2.UserName = "e.rice@bevosbooks.com";
                m2.PhoneNumber = "2706602803";
                await _userManager.AddToRoleAsync(m2, "Manager");
                await _userManager.CreateAsync(m2, "arched");
                Managers.Add(m2);

                AppUser m3 = new AppUser();
                m3.PasswordHash = "lottery";
                m3.LastName = "Rogers";
                m3.FirstName = "Allen";
                m3.Address = "4965 Oak Hill";
                m3.City = "Austin";
                m3.State = "TX";
                m3.Zipcode = "78705";
                m3.Email = "a.rogers@bevosbooks.com";
                m3.UserName = "a.rogers@bevosbooks.com";
                m3.PhoneNumber = "4139645586";
                await _userManager.AddToRoleAsync(m3, "Manager");
                await _userManager.CreateAsync(m3, "lottery");
                Managers.Add(m3);

                AppUser m4 = new AppUser();
                m4.PasswordHash = "offbeat";
                m4.LastName = "Sewell";
                m4.FirstName = "William";
                m4.Address = "2365 51st St.";
                m4.City = "Austin";
                m4.State = "TX";
                m4.Zipcode = "78755";
                m4.Email = "w.sewell@bevosbooks.com";
                m4.UserName = "w.sewell@bevosbooks.com";
                m4.PhoneNumber = "7224308314";
                await _userManager.AddToRoleAsync(m4, "Manager");
                await _userManager.CreateAsync(m4, "offbeat");
                Managers.Add(m4);


                AppUser m5 = new AppUser();
                m5.PasswordHash = "landus";
                m5.LastName = "Taylor";
                m5.FirstName = "Rachel";
                m5.Address = "345 Longview Dr.";
                m5.City = "Austin";
                m5.State = "TX";
                m5.Zipcode = "78746";
                m5.Email = "r.taylor@bevosbooks.com";
                m5.UserName = "r.taylor@bevosbooks.com";
                m5.PhoneNumber = "9071236087";
                await _userManager.AddToRoleAsync(m5, "Manager");
                await _userManager.CreateAsync(m5, "landus");
                Managers.Add(m5);


            }

            catch
            {
                //
            }

            _db.SaveChanges();



        }
    }
}
  