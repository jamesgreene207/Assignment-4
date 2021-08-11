using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext _context;

        public HomeController(JobDbContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = _context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet]
        public IActionResult AddJob()
        {
            List<Skill> skills = _context.Skills.ToList();
            List<Employer> employers = _context.Employers.ToList();
            AddJobViewModel addJobViewModel = new AddJobViewModel(employers, skills);


            return View(addJobViewModel);
        }

        [HttpPost]
        public IActionResult AddJob(AddJobViewModel addJobViewModel, Skill[] selectedSkills)
        {           

            if (ModelState.IsValid)
            {
                Employer theEmployer = _context.Employers.Find(addJobViewModel.EmployerId);
                Job newJob = new Job(addJobViewModel.Name, theEmployer);
                foreach(var skill in selectedSkills)
                {
                    JobSkill jobSkill = new JobSkill(newJob, skill);
                    _context.JobSkills.Add(jobSkill);
                }
                    
                _context.Jobs.Add(newJob);
                _context.SaveChanges();

                return Redirect("/List");
             
            }
            return View(addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = _context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = _context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
