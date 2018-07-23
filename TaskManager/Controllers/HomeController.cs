﻿using AutoMapper;
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

        [HttpGet]
        public async Task<ActionResult> ListProject()
        {
            List<ListProjectViewModel> listProjectViewModel = new List<ListProjectViewModel>();
            var projects = await context.Projects.ToListAsync();
            var model = Mapper.Map(projects, listProjectViewModel);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> CreateProject()
        {
            var items = await context.Users.ToListAsync();
            var projectManager = new SelectList((from s in items
                                                 select new
                                                 {
                                                     s.Id,
                                                     Name = s.UserData.LastName + " " + s.UserData.FirstName + " " + s.UserData.MiddleName
                                                 }), "Id", "Name", null);
            
            ViewBag.Items = projectManager;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = Mapper.Map<CreateProjectViewModel, Project>(model);
                context.Projects.Add(project);
                await context.SaveChangesAsync();
                return RedirectToAction("ListProject");
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ProjectEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var project = await context.Projects.FindAsync(id);

            if (project != null)
            {

                var items = await context.Users.ToListAsync();

                var projectManager = new SelectList((from s in items
                                                         select new
                                                         {
                                                             s.Id,
                                                             Name = s.UserData.LastName + " " + s.UserData.FirstName + " " + s.UserData.MiddleName
                                                         }), "Id", "Name", project.UserId);
                ViewBag.Items = projectManager;
                var projectEdit = Mapper.Map<Project, EditProjectViewModel>(project);

                if (projectEdit.DateClose != null)
                {
                    projectEdit.ProjectClose = true;
                }
                return View(projectEdit);
            }

            return RedirectToAction("ListProject");
        }

        [HttpPost]
        public async Task<ActionResult> ProjectEdit(EditProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ProjectClose == true && model.DateClose == null)
                {
                    model.DateClose = DateTime.Now;
                }
                else if (model.ProjectClose == false && model.DateClose != null)
                {
                    model.DateClose = null;
                }

                var project = Mapper.Map<EditProjectViewModel, Project>(model);

                context.Entry(project).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return RedirectToAction("ListProject");
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ListTask(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            List<ListTaskViewModel> listTaskViewModel = new List<ListTaskViewModel>();

            var tasks = await context.Tasks.Where(m => m.ProjectId == id).ToListAsync();

            if (tasks != null)
            {
                var projectName = await context.Projects.FirstOrDefaultAsync(m => m.Id == id);
                var model = Mapper.Map(tasks, listTaskViewModel);

                ViewBag.Project = projectName.Name;

                return View(model);
            }

            return RedirectToAction("ListProject");
        }
        

        // Допилить метод
        [HttpGet]
        public async Task<ActionResult> TaskEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var task = await context.Tasks.FindAsync(id);
            if (task != null)
            {
                //var taskEdit = Mapper.Map<Task, EditTaskViewModel>(task);
                //return View(taskEdit);
            }
            return RedirectToAction("ListProject");
        }

        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }
    }
}