﻿using System;
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
        private JobDbContext context;

        public EmployerController(JobDbContext dbContext)
        {
            context = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Employer> employers = context.Employers.ToList();

            return View(employers);
        }

        public IActionResult Add()
        {
            AddEmployerViewModel addemployerviewmodel = new AddEmployerViewModel();
            return View(addemployerviewmodel);
        }
       
        [HttpPost]
        public IActionResult ProcessAddEmployerForm(AddEmployerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Employer newemployer = new Employer
                {
                    Name = viewModel.Name,
                    Location = viewModel.Location
                };

                context.Employers.Add(newemployer);
                context.SaveChanges();

                return Redirect("/");
            }

            return View(viewModel);
        }

        public IActionResult About(int id)
        {

            Employer employer = context.Employers.Find(id);
            AddEmployerViewModel viewModel = new AddEmployerViewModel
            {
                Location = employer.Location,
                Name = employer.Name
            };
            return View(viewModel);
        }
    }
}
