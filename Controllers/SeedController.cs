using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Group14_BevoBooks.DAL;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group14_BevoBooks.Controllers
{     public class SeedController : Controller     {         private AppDbContext _db;
        private IServiceProvider _service;          public SeedController(AppDbContext context, IServiceProvider service)         {             _db = context;
            _service = service;         }

        // GET: /<controller>/
        public IActionResult Index()         {             return View();         }          public IActionResult SeedBooks()         {             try             {                 Seeding.SeedBooks.SeedAllBooks(_db);             }             catch (NotSupportedException ex)             {                 return View("Error", new String[] { "The data has already been added", ex.Message });             }             catch (InvalidOperationException ex)             {                 return View("Error", new String[] { "There was an error adding data to the database", ex.Message });             }              return View("Confirm");         }

        public IActionResult SeedGenres()
        {
            try
            {
                Seeding.SeedGenres.SeedAllGenres(_db);
            }
            catch (NotSupportedException ex)
            {
                return View("Error", new String[] { "The data has already been added", ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return View("Error", new String[] { "There was an error adding data to the database", ex.Message });
            }

            return View("Confirm");
        }

        public IActionResult SeedIdentity()
        {
            try
            {
                Seeding.SeedIdentity.AddAdmin(_service).Wait();
            }
            catch (NotSupportedException ex)
            {
                return View("Error", new String[] { "The data has already been added", ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return View("Error", new String[] { "There was an error adding data to the database", ex.Message });
            }

            return View("Confirm");
        }

        public IActionResult SeedManagers()
        {
            try
            {
                Seeding.SeedManagers.SeedAllManagersAsync(_service).Wait();
            }
            catch (NotSupportedException ex)
            {
                return View("Error", new String[] { "The data has already been added", ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return View("Error", new String[] { "There was an error adding data to the database", ex.Message });
            }

            return View("Confirm");
        }

        public IActionResult SeedEmployees()
        {
            try
            {
                Seeding.SeedEmployees.SeedAllEmployeesAsync(_service).Wait();
            }
            catch (NotSupportedException ex)
            {
                return View("Error", new String[] { "The data has already been added", ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return View("Error", new String[] { "There was an error adding data to the database", ex.Message });
            }

            return View("Confirm");
        }

        public IActionResult SeedCustomers()
        {
            try
            {
                Seeding.SeedCustomers.SeedAllCustomersAsync(_service).Wait();
            }
            catch (NotSupportedException ex)
            {
                return View("Error", new String[] { "The data has already been added", ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return View("Error", new String[] { "There was an error adding data to the database", ex.Message });
            }

            return View("Confirm");
        }
    }
} 
