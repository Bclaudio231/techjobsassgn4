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
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();

            List<Skill> skills = context.Skills.ToList();
            AddJobViewModel addjobviewmodel = new AddJobViewModel(employers, skills);
            return View(addjobviewmodel);
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel viewModel, string [] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                Job newjob = new Job
                {
                    Name = viewModel.Name,
                    EmployerId = viewModel.EmployerId
                };

                context.Jobs.Add(newjob);
                foreach(string skillid in selectedSkills)
                {
                    int id = Convert.ToInt32(skillid);
                    JobSkill jobskill = new JobSkill
                    {
                        Job = newjob,
                        SkillId = id
                    };
                    context.JobSkills.Add(jobskill);
                }
                context.SaveChanges();

                return Redirect("/");
            }

            return View(viewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
