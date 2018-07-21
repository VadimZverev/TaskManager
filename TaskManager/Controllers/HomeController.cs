using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskManager.DAL;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private DataContext context = new DataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CreateProject()
        {
            var items = await context.Users.ToListAsync();
            var projectManager = new SelectList((from s in items
                                                 select new
                                                 {
                                                     s.Id,
                                                     Name = s.UserData.LastName + " " + s.UserData.MiddleName + " " + s.UserData.FirstName
                                                 }), "Id", "Name", null);
            
            ViewBag.Items = projectManager;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject(CreateProjectView model)
        {
            if (ModelState.IsValid)
            {
                var project = Mapper.Map<CreateProjectView, Project>(model);
                context.Projects.Add(project);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }
    }
}