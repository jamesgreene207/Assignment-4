using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechJobsPersistent.Data;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistent.Controllers
{
    public class EmployerController : Controller
    {
        
        private JobDbContext _context;

        public EmployerController(JobDbContext dbContext)
        {
            _context = dbContext;
        }
        public IActionResult Index()
        {
            List<Employer> employers = _context.Employers.ToList();
            return View(employers);
        }

        
        public IActionResult Add()
        {
            AddEmployerViewModel addEmployerViewModel = new AddEmployerViewModel();
            return View(addEmployerViewModel);
        }
        [HttpPost]
        public IActionResult Add(AddEmployerViewModel addEmployerViewModel)
        {
            if (ModelState.IsValid)
            {

                string name = addEmployerViewModel.Name;
                string location = addEmployerViewModel.Location;

                List<Employer> existingItems = _context.Employers
                    .Where(e => e.Name == name)
                    .Where(e => e.Location == location)
                    .ToList();

                if (existingItems.Count == 0)
                {
                    Employer employer = new Employer
                    {
                        Name = name,
                        Location = location
                    };
                    _context.Employers.Add(employer);
                    _context.SaveChanges();

                    return Redirect("/Employer/Index");
                }
            }
                return View(addEmployerViewModel);
        }

        public IActionResult About(int id)
        {
            Employer employer = _context.Employers.Find(id);
                

            return View(employer);
        }
    }
}
