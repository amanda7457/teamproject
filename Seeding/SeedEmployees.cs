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
    public class SeedEmployees
    {

        public static async Task SeedAllEmployeesAsync(IServiceProvider serviceProvider)
        {
            AppDbContext _db = serviceProvider.GetRequiredService<AppDbContext>();
            UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            //Int32 intCustomersAdded = 0;
            //String CustomerName = "Begin"; //helps to keep track of error on books
            List<AppUser> Employees = new List<AppUser>();

            
            {
                AppUser e1 = new AppUser();
                e1.PasswordHash = "evanescent";
                e1.LastName = "Barnes";
                e1.FirstName = "Susan";
                e1.Address = "888 S. Main";
                e1.City = "Kyle";
                e1.State = "TX";
                e1.Zipcode = "78640";
                e1.Email = "s.barnes@bevosbooks.com";
                e1.UserName = "s.barnes@bevosbooks.com";
                e1.PhoneNumber = "9636389416";
                await _userManager.AddToRoleAsync(e1, "Employee");
                await _userManager.CreateAsync(e1, "evanescent");
                Employees.Add(e1);

                AppUser e2 = new AppUser();
                e2.PasswordHash = "squirrel";
                e2.LastName = "Garcia";
                e2.FirstName = "Hector";
                e2.Address = "777 PBR Drive";
                e2.City = "Austin";
                e2.State = "TX";
                e2.Zipcode = "78712";
                e2.Email = "h.garcia@bevosbooks.com";
                e2.UserName = "h.garcia@bevosbooks.com";
                e2.PhoneNumber = "4547135738";
                await _userManager.AddToRoleAsync(e2, "Employee");
                await _userManager.CreateAsync(e2, "squirrel");
                Employees.Add(e2);

                AppUser e3 = new AppUser();
                e3.PasswordHash = "changalang";
                e3.LastName = "Ingram";
                e3.FirstName = "Brad";
                e3.Address = "6548 La Posada Ct.";
                e3.City = "Austin";
                e3.State = "TX";
                e3.Zipcode = "78705";
                e3.Email = "b.ingram@bevosbooks.com";
                e3.UserName = "b.ingram@bevosbooks.com";
                e3.PhoneNumber = "5817343315";
                await _userManager.AddToRoleAsync(e3, "Employee");
                await _userManager.CreateAsync(e3, "changalang");
                Employees.Add(e3);

                AppUser e4 = new AppUser();
                e4.PasswordHash = "rhythm";
                e4.LastName = "Jack";
                e4.FirstName = "Jackson";
                e4.Address = "222 Main";
                e4.City = "Austin";
                e4.State = "TX";
                e4.Zipcode = "78760";
                e4.Email = "j.jackson@bevosbooks.com";
                e4.UserName = "j.jackson@bevosbooks.com";
                e4.PhoneNumber = "8241915317";
                await _userManager.AddToRoleAsync(e4, "Employee");
                await _userManager.CreateAsync(e4, "rhythm");
                Employees.Add(e4);

                AppUser e5 = new AppUser();
                e5.PasswordHash = "approval";
                e5.LastName = "Jacobs";
                e5.FirstName = "Todd";
                e5.Address = "4564 Elm St.";
                e5.City = "Georgetown";
                e5.State = "TX";
                e5.Zipcode = "78628";
                e5.Email = "t.jacobs@bevosbooks.com";
                e5.UserName = "t.jacobs@bevosbooks.com";
                e5.PhoneNumber = "2477822475";
                await _userManager.AddToRoleAsync(e5, "Employee");
                await _userManager.CreateAsync(e5, "approval");
                Employees.Add(e5);

                AppUser e6 = new AppUser();
                e6.PasswordHash = "society";
                e6.LastName = "Jones";
                e6.FirstName = "Lester";
                e6.Address = "999 LeBlat";
                e6.City = "Austin";
                e6.State = "TX";
                e6.Zipcode = "78747";
                e6.Email = "l.jones@bevosbooks.com";
                e6.UserName = "l.jones@bevosbooks.com";
                e6.PhoneNumber = "4764966462";
                await _userManager.AddToRoleAsync(e6, "Employee");
                await _userManager.CreateAsync(e6, "society");
                Employees.Add(e6);

                AppUser e7 = new AppUser();
                e7.PasswordHash = "tanman";
                e7.LastName = "Larson";
                e7.FirstName = "Bill";
                e7.Address = "1212 N. First Ave";
                e7.City = "Round Rock";
                e7.State = "TX";
                e7.Zipcode = "78665";
                e7.Email = "b.larson@bevosbooks.com";
                e7.UserName = "b.larson@bevosbooks.com";
                e7.PhoneNumber = "3355258855";
                await _userManager.AddToRoleAsync(e7, "Employee");
                await _userManager.CreateAsync(e7, "tanman");
                Employees.Add(e7);

                AppUser e8 = new AppUser();
                e8.PasswordHash = "longhorns";
                e8.LastName = "Lawrence";
                e8.FirstName = "Victoria";
                e8.Address = "6639 Bookworm Ln.";
                e8.City = "Austin";
                e8.State = "TX";
                e8.Zipcode = "78712";
                e8.Email = "v.lawrence@bevosbooks.com";
                e8.UserName = "v.lawrence@bevosbooks.com";
                e8.PhoneNumber = "7511273054";
                await _userManager.AddToRoleAsync(e8, "Employee");
                await _userManager.CreateAsync(e8, "longhorns");
                Employees.Add(e8);

                AppUser e9 = new AppUser();
                e9.PasswordHash = "swansong";
                e9.LastName = "Lopez";
                e9.FirstName = "Marshall";
                e9.Address = "90 SW North St";
                e9.City = "Austin";
                e9.State = "TX";
                e9.Zipcode = "78729";
                e9.Email = "m.lopez@bevosbooks.com";
                e9.UserName = "m.lopez@bevosbooks.com";
                e9.PhoneNumber = "7477907070";
                await _userManager.AddToRoleAsync(e9, "Employee");
                await _userManager.CreateAsync(e9, "swansong");
                Employees.Add(e9);

                AppUser e10 = new AppUser();
                e10.PasswordHash = "fungus";
                e10.LastName = "MacLeod";
                e10.FirstName = "Jennifer";
                e10.Address = "2504 Far West Blvd.";
                e10.City = "Austin";
                e10.State = "TX";
                e10.Zipcode = "78705";
                e10.Email = "j.macleod@bevosbooks.com";
                e10.UserName = "j.macleod@bevosbooks.com";
                e10.PhoneNumber = "2621216845";
                await _userManager.AddToRoleAsync(e10, "Employee");
                await _userManager.CreateAsync(e10, "fungus");
                Employees.Add(e10);

                AppUser e11 = new AppUser();
                e11.PasswordHash = "median";
                e11.LastName = "Markham";
                e11.FirstName = "Elizabeth";
                e11.Address = "7861 Chevy Chase";
                e11.City = "Austin";
                e11.State = "TX";
                e11.Zipcode = "78785";
                e11.Email = "j.macleod@bevosbooks.com";
                e11.UserName = "j.macleod@bevosbooks.com";
                e11.PhoneNumber = "5028075807";
                await _userManager.AddToRoleAsync(e11, "Employee");
                await _userManager.CreateAsync(e11, "median");
                Employees.Add(e11);

                AppUser e12 = new AppUser();
                e12.PasswordHash = "decorate";
                e12.LastName = "Martinez";
                e12.FirstName = "Gregory";
                e12.Address = "8295 Sunset Blvd.";
                e12.City = "Austin";
                e12.State = "TX";
                e12.Zipcode = "78712";
                e12.Email = "g.martinez@bevosbooks.com";
                e12.UserName = "g.martinez@bevosbooks.com";
                e12.PhoneNumber = "1994708542";
                await _userManager.AddToRoleAsync(e12, "Employee");
                await _userManager.CreateAsync(e12, "decorate");
                Employees.Add(e12);

                AppUser e13= new AppUser();
                e13.PasswordHash = "rankmary";
                e13.LastName = "Mason";
                e13.FirstName = "Jack";
                e13.Address = "444 45th St";
                e13.City = "Austin";
                e13.State = "TX";
                e13.Zipcode = "78701";
                e13.Email = "j.mason@bevosbooks.com";
                e13.UserName = "j.mason@bevosbooks.com";
                e13.PhoneNumber = "1748136441";
                await _userManager.AddToRoleAsync(e13, "Employee");
                await _userManager.CreateAsync(e13, "rankmary");
                Employees.Add(e13);

                AppUser e14 = new AppUser();
                e14.PasswordHash = "kindly";
                e14.LastName = "Miller";
                e14.FirstName = "Charles";
                e14.Address = "8962 Main St.";
                e14.City = "Austin";
                e14.State = "TX";
                e14.Zipcode = "78709";
                e14.Email = "c.miller@bevosbooks.com";
                e14.UserName = "c.miller@bevosbooks.com";
                e14.PhoneNumber = "8999319585";
                await _userManager.AddToRoleAsync(e14, "Employee");
                await _userManager.CreateAsync(e14, "kindly");
                Employees.Add(e14);

                AppUser e15 = new AppUser();
                e15.PasswordHash = "ricearoni";
                e15.LastName = "Nguyen";
                e15.FirstName = "Mary";
                e15.Address = "465 N. Bear Cub";
                e15.City = "Austin";
                e15.State = "TX";
                e15.Zipcode = "78734";
                e15.Email = "m.nguyen@bevosbooks.com";
                e15.UserName = "m.nguyen@bevosbooks.com";
                e15.PhoneNumber = "8716746381";
                await _userManager.AddToRoleAsync(e15, "Employee");
                await _userManager.CreateAsync(e15, "ricearoni");
                Employees.Add(e15);

                AppUser e16 = new AppUser();
                e16.PasswordHash = "walkamile";
                e16.LastName = "Rankin";
                e16.FirstName = "Suzie";
                e16.Address = "23 Dewey Road";
                e16.City = "Austin";
                e16.State = "TX";
                e16.Zipcode = "78712";
                e16.Email = "s.rankin@bevosbooks.com";
                e16.UserName = "s.rankin@bevosbooks.com";
                e16.PhoneNumber = "5239029525";
                await _userManager.AddToRoleAsync(e16, "Employee");
                await _userManager.CreateAsync(e16, "walkamile");
                Employees.Add(e16);

                AppUser e17 = new AppUser();
                e17.PasswordHash = "ingram45";
                e17.LastName = "Rhodes";
                e17.FirstName = "Megan";
                e17.Address = "4587 Enfield Rd.";
                e17.City = "Austin";
                e17.State = "TX";
                e17.Zipcode = "78729";
                e17.Email = "m.rhodes@bevosbooks.com";
                e17.UserName = "m.rhodes@bevosbooks.com";
                e17.PhoneNumber = "1232139514";
                await _userManager.AddToRoleAsync(e17, "Employee");
                await _userManager.CreateAsync(e17, "ingram45");
                Employees.Add(e17);

                AppUser e18 = new AppUser();
                e18.PasswordHash = "nostalgic";
                e18.LastName = "Saunders";
                e18.FirstName = "Sarah";
                e18.Address = "332 Avenue C";
                e18.City = "Austin";
                e18.State = "TX";
                e18.Zipcode = "78733";
                e18.Email = "s.saunders@bevosbooks.com";
                e18.UserName = "s.saunders@bevosbooks.com";
                e18.PhoneNumber = "9036349587";
                await _userManager.AddToRoleAsync(e18, "Employee");
                await _userManager.CreateAsync(e18, "nostalgic");
                Employees.Add(e18);

                AppUser e19 = new AppUser();
                e19.PasswordHash = "evanescent";
                e19.LastName = "Sheffield";
                e19.FirstName = "Martin";
                e19.Address = "3886 Avenue A";
                e19.City = "San Marcos";
                e19.State = "TX";
                e19.Zipcode = "78666";
                e19.Email = "m.sheffield@bevosbooks.com";
                e19.UserName = "m.sheffield@bevosbooks.com";
                e19.PhoneNumber = "9349192978";
                await _userManager.AddToRoleAsync(e19, "Employee");
                await _userManager.CreateAsync(e19, "evanescent");
                Employees.Add(e19);

                AppUser e20 = new AppUser();
                e20.PasswordHash = "stewboy";
                e20.LastName = "Silva";
                e20.FirstName = "Cindy";
                e20.Address = "900 4th St";
                e20.City = "Austin";
                e20.State = "TX";
                e20.Zipcode = "78758";
                e20.Email = "c.silva@bevosbooks.com";
                e20.UserName = "c.silva@bevosbooks.com";
                e20.PhoneNumber = "4874328170";
                await _userManager.AddToRoleAsync(e20, "Employee");
                await _userManager.CreateAsync(e20, "stewboy");
                Employees.Add(e20);

                AppUser e21 = new AppUser();
                e21.PasswordHash = "instrument";
                e21.LastName = "Stuart";
                e21.FirstName = "Eric";
                e21.Address = "5576 Toro Ring";
                e21.City = "Austin";
                e21.State = "TX";
                e21.Zipcode = "78758";
                e21.Email = "e.stuart@bevosbooks.com";
                e21.UserName = "e.stuart@bevosbooks.com";
                e21.PhoneNumber = "1967846827";
                await _userManager.AddToRoleAsync(e21, "Employee");
                await _userManager.CreateAsync(e21, "instrument");
                Employees.Add(e21);

                AppUser e22 = new AppUser();
                e22.PasswordHash = "hecktour";
                e22.LastName = "Tanner";
                e22.FirstName = "Jeremy";
                e22.Address = "4347 Almstead";
                e22.City = "Austin";
                e22.State = "TX";
                e22.Zipcode = "78712";
                e22.Email = "j.tanner@bevosbooks.com";
                e22.UserName = "j.tanner@bevosbooks.com";
                e22.PhoneNumber = "5923026779";
                await _userManager.AddToRoleAsync(e22, "Employee");
                await _userManager.CreateAsync(e22, "hecktour");
                Employees.Add(e22);

                AppUser e23 = new AppUser();
                e23.PasswordHash = "countryrhodes";
                e23.LastName = "Taylor";
                e23.FirstName = "Allison";
                e23.Address = "467 Nueces St.";
                e23.City = "Austin";
                e23.State = "TX";
                e23.Zipcode = "78727";
                e23.Email = "a.taylor@bevosbooks.com";
                e23.UserName = "a.taylor@bevosbooks.com";
                e23.PhoneNumber = "7246195827";
                await _userManager.AddToRoleAsync(e23, "Employee");
                await _userManager.CreateAsync(e23, "countryrhodes");
                Employees.Add(e23);






            }
            _db.SaveChanges();
        }
            
    }
    
}
