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

        [HttpPost]
        public async Task<ActionResult> ProjectSearch(string name)
        {
            List<ListProjectViewModel> listProjectViewModel = new List<ListProjectViewModel>();
            var allProject = await context.Projects.Where(m => m.User.UserData.FirstName.Contains(name)).ToListAsync();
            var model = Mapper.Map(allProject, listProjectViewModel);

            return PartialView(model);
        }

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
        public ActionResult ProjectEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var project = context.Projects.Find(id);

            if (project != null)
            {

                var items = context.Users.ToList();

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
                return PartialView(projectEdit);
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

        public ActionResult ListTask(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            List<ListTaskViewModel> listTaskViewModel = new List<ListTaskViewModel>();

            var tasks = context.Tasks.Where(m => m.ProjectId == id).ToList();

            if (tasks != null)
            {
                var projectName = context.Projects.FirstOrDefault(m => m.Id == id);
                var model = Mapper.Map(tasks, listTaskViewModel);

                ViewBag.Project = projectName.Name;

                return PartialView(model);
            }

            return RedirectToAction("ListProject");
        }

        [HttpGet]
        public async Task<ActionResult> CreateTask( int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListProject");
            }

            ViewBag.ProjectId = id;

            var types = await context.TaskTypes.ToListAsync();
            var taskTypes = new SelectList(types, "Id", "Name");
            ViewBag.TaskTypes = taskTypes;

            var priorities = await context.TaskPriorities.ToListAsync();
            var taskPriorities = new SelectList(priorities, "Id", "Name");
            ViewBag.TaskPriorities = taskPriorities;

            var users = await context.Users.ToListAsync();
            var taskUser = new SelectList((from s in users
                                           select new
                                           {
                                               s.Id,
                                               Name = s.UserData.LastName + " " 
                                               + s.UserData.FirstName + " " 
                                               + s.UserData.MiddleName
                                           }), "Id", "Name");
            ViewBag.TaskUser = taskUser;

            var statuses = await context.TaskStatuses.ToListAsync();
            var taskStatuses = new SelectList(statuses, "Id", "Name");
            ViewBag.TaskStatuses = taskStatuses;

            return View();
            
        }

        [HttpPost]
        public async Task<ActionResult> CreateTask(CreateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = Mapper.Map<CreateTaskViewModel, DAL.Task>(model);

                context.Tasks.Add(task);
                await context.SaveChangesAsync();

                return RedirectToAction("ListTask", new { id = model.ProjectId });
            }
            return View();
        }

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
                var types = await context.TaskTypes.ToListAsync();
                var taskTypes = new SelectList(types, "Id", "Name", task.TaskTypeId);
                ViewBag.TaskTypes = taskTypes;

                var priorities = await context.TaskPriorities.ToListAsync();
                var taskPriorities = new SelectList(priorities, "Id", "Name", task.TaskPriorityId);
                ViewBag.TaskPriorities = taskPriorities;

                var users = await context.Users.ToListAsync();
                var taskUser = new SelectList((from s in users
                                                     select new
                                                     {
                                                         s.Id,
                                                         Name = s.UserData.LastName + " " + s.UserData.FirstName + " " + s.UserData.MiddleName
                                                     }), "Id", "Name", task.UserId);
                ViewBag.TaskUser = taskUser;

                var statuses = await context.TaskStatuses.ToListAsync();
                var taskStatuses = new SelectList(statuses, "Id", "Name", task.TaskStatusId);
                ViewBag.TaskStatuses = taskStatuses;



                var taskEdit = Mapper.Map<DAL.Task, EditTaskViewModel>(task);

                if (taskEdit.DateClose != null)
                {
                    taskEdit.TaskClose = true;
                }
                return View(taskEdit);
            }
            return RedirectToAction("ListTask", new { id = task.ProjectId });
        }

        [HttpPost]
        public async Task<ActionResult> TaskEdit(EditTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TaskClose == true && model.DateClose == null)
                {
                    model.DateClose = DateTime.Now;
                }
                else if (model.TaskClose == false && model.DateClose != null)
                {
                    model.DateClose = null;
                }

                var task = Mapper.Map<EditTaskViewModel, DAL.Task>(model);

                context.Entry(task).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return RedirectToAction("ListTask", new { id = model.ProjectId });
            }

            return View(model);

        }

        // Надо допилить.
        [HttpPost]
        public JsonResult TaskDelete(int id)
        {
            try
            {
                var task = context.Tasks.Where(x => x.Id == id).FirstOrDefault();

                if (task == null)
                {
                    return Json(new { result = false });
                }

                context.Tasks.Remove(task);
                context.SaveChanges();

                return Json(new { result = true });
            }
            catch (Exception exc)
            {
                return Json(exc.Message);
            }
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