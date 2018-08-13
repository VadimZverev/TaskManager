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

        // Тестовый метод поиска проектов через поисковик.
        [HttpPost]
        public ActionResult ProjectSearch(string name)
        {
            List<ListProjectViewModel> listProjectViewModel = new List<ListProjectViewModel>();
            var allProject = context.Projects.Where(m => m.User.UserData.FirstName.Contains(name)).ToList();
            var model = Mapper.Map(allProject, listProjectViewModel);

            return PartialView(model);
        }

        // Открытие списка проектов
        [Authorize]
        public ActionResult ListProject()
        {
            List<ListProjectViewModel> listProjectViewModel = new List<ListProjectViewModel>();
            var projects = context.Projects.ToList();
            var model = Mapper.Map(projects, listProjectViewModel);
            return View(model);
        }

        // Открытие окна создания проекта
        public ActionResult CreateProject()
        {
            var items = context.Users.Where(m => m.Role.Name.Contains("Project Manager")).ToList();

            Session["Items"] = new SelectList(
                (from s in items
                 select new
                 {
                     s.Id,
                     Name = s.UserData.LastName + " " + s.UserData.FirstName + " " + s.UserData.MiddleName
                 }), "Id", "Name", null);


            return PartialView();
        }

        // Сохранение созданного проекта.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateProject(CreateProjectViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var project = Mapper.Map<CreateProjectViewModel, Project>(model);
                    var user = context.UserDatas.Find(model.UserId);
                    context.Projects.Add(project);
                    context.SaveChanges();

                    Session.Clear();

                    return Json(new
                    {
                        ProjectId = project.Id,
                        ProjectName = model.Name,
                        ProjectManager = user.LastName + " " +
                                        user.FirstName + " " +
                                        user.MiddleName,
                        DateCreate = project.DateCreate.ToShortDateString(),
                        result = true
                    });
                }

                return Json(new { result = false });
            }
            catch (Exception exc)
            {
                return Json(new { exc.Message });
            }

        }

        // Открытие окна редактирование проекта
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
                var items = context.Users.Where(m => m.Role.Name.Contains("Project Manager")).ToList();

                Session["projectUser"] = new SelectList((from s in items
                                                         select new
                                                         {
                                                             s.Id,
                                                             Name = s.UserData.LastName + " "
                                                             + s.UserData.FirstName + " "
                                                             + s.UserData.MiddleName
                                                         }), "Id", "Name", project.UserId);

                var projectEdit = Mapper.Map<Project, EditProjectViewModel>(project);

                if (projectEdit.DateClose != null)
                {
                    projectEdit.ProjectClose = true;
                }
                return PartialView(projectEdit);
            }

            return RedirectToAction("ListProject");
        }

        // Сохранение изменений проекта.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ProjectEdit(EditProjectViewModel model)
        {
            try
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
                    var user = context.Users.Find(model.UserId);

                    context.Entry(project).State = EntityState.Modified;
                    context.SaveChanges();

                    Session.Clear();

                    return Json(new
                    {
                        ProjectId = project.Id,
                        ProjectName = model.Name,
                        ProjectManager = user.UserData.LastName + " " +
                                        user.UserData.FirstName + " " +
                                        user.UserData.MiddleName,
                        DateCreate = project.DateCreate.ToShortDateString(),
                        DateClose = project.DateClose.HasValue ? project.DateClose.Value.ToShortDateString() : "",
                        result = true
                    });

                }

                return Json(new { result = false });
            }
            catch (Exception exc)
            {
                return Json(new { exc.Message });
            }
        }

        // Удаление проекта
        public JsonResult ProjectDelete(int id)
        {
            try
            {
                var project = context.Projects.Where(x => x.Id == id).FirstOrDefault();

                if (project == null)
                {
                    return Json(new { result = false });
                }

                context.Projects.Remove(project);
                context.SaveChanges();

                return Json(new { result = true });
            }
            catch (Exception exc)
            {
                return Json(exc.Message);
            }
        }

        // Список задач по проекту.
        public ActionResult ListTask(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            List<ListTaskViewModel> listTaskViewModel = new List<ListTaskViewModel>();

            var tasks = context.Tasks.Where(m => m.ProjectId == id).ToList();

            var projectName = context.Projects.FirstOrDefault(m => m.Id == id);
            var model = Mapper.Map(tasks, listTaskViewModel);

            ViewBag.Project = projectName.Name;
            ViewBag.ProjectId = projectName.Id;
            return PartialView(model);

        }

        // Открытие окна создания задачи.
        [HttpGet]
        public async Task<ActionResult> CreateTask(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListProject");
            }

            Session["projectId"] = id;

            var types = await context.TaskTypes.ToListAsync();
            Session["taskTypes"] = new SelectList(types, "Id", "Name");

            var priorities = await context.TaskPriorities.ToListAsync();
            Session["taskPriorities"] = new SelectList(priorities, "Id", "Name");

            var users = await context.Users.ToListAsync();
            Session["taskUser"] = new SelectList((from s in users
                                                  select new
                                                  {
                                                      s.Id,
                                                      Name = s.UserData.LastName + " "
                                                      + s.UserData.FirstName + " "
                                                      + s.UserData.MiddleName
                                                  }), "Id", "Name");

            var statuses = await context.TaskStatuses.ToListAsync();
            Session["taskStatuses"] = new SelectList(statuses, "Id", "Name");

            return PartialView();

        }

        // сохранение созданной задачи.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateTask(CreateTaskViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var task = Mapper.Map<CreateTaskViewModel, DAL.Task>(model);
                    var type = await context.TaskTypes.FindAsync(model.TaskTypeId);
                    var priority = await context.TaskPriorities.FindAsync(model.TaskPriorityId);
                    var user = await context.UserDatas.FindAsync(model.UserId);
                    var status = await context.TaskStatuses.FindAsync(model.TaskStatusId);

                    context.Tasks.Add(task);
                    await context.SaveChangesAsync();

                    Session.Clear();

                    return Json(new
                    {
                        taskId = task.Id,
                        taskName = model.TaskName,
                        taskType = type.Name,
                        description = model.Description,
                        taskPriority = priority.Name,
                        taskUser = user.LastName + " " +
                                        user.FirstName + " " +
                                        user.MiddleName,
                        taskStatus = status.Name,
                        DateCreate = task.DateCreate.ToShortDateString(),
                        result = true
                    });
                }

                return Json(new { result = false });
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    exc.Message
                });
            }

        }

        // Открытие окна редактирование задачи.
        [HttpGet]
        public async Task<ActionResult> TaskEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListProject");
            }

            var task = await context.Tasks.FindAsync(id);

            if (task != null)
            {
                var types = await context.TaskTypes.ToListAsync();
                Session["taskTypes"] = new SelectList(types, "Id", "Name", task.TaskTypeId);

                var priorities = await context.TaskPriorities.ToListAsync();
                Session["taskPriorities"] = new SelectList(priorities, "Id", "Name", task.TaskPriorityId);

                var users = await context.Users.ToListAsync();
                Session["taskUser"] = new SelectList((from s in users
                                                      select new
                                                      {
                                                          s.Id,
                                                          Name = s.UserData.LastName + " "
                                                          + s.UserData.FirstName + " "
                                                          + s.UserData.MiddleName
                                                      }), "Id", "Name", task.UserId);

                var statuses = await context.TaskStatuses.ToListAsync();
                Session["taskStatuses"] = new SelectList(statuses, "Id", "Name", task.TaskStatusId);

                var taskEdit = Mapper.Map<DAL.Task, EditTaskViewModel>(task);

                if (taskEdit.DateClose != null)
                {
                    taskEdit.TaskClose = true;
                }

                return PartialView(taskEdit);
            }
            return RedirectToAction("ListTask", new { id = task.ProjectId });
        }

        // Сохранение изменений задачи.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> TaskEdit(EditTaskViewModel model)
        {
            try
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

                    var type = await context.TaskTypes.FindAsync(model.TaskTypeId);
                    var priority = await context.TaskPriorities.FindAsync(model.TaskPriorityId);
                    var user = await context.UserDatas.FindAsync(model.UserId);
                    var status = await context.TaskStatuses.FindAsync(model.TaskStatusId);

                    context.Entry(task).State = EntityState.Modified;

                    await context.SaveChangesAsync();

                    Session.Clear();

                    return Json(new
                    {
                        taskId = task.Id,
                        taskName = model.TaskName,
                        taskType = type.Name,
                        description = model.Description,
                        taskPriority = priority.Name,
                        taskUser = user.LastName + " " +
                                        user.FirstName + " " +
                                        user.MiddleName,
                        taskStatus = status.Name,
                        DateCreate = model.DateCreate.ToShortDateString(),
                        DateClose = model.DateClose.HasValue ? model.DateClose.Value.ToShortDateString() : "",
                        result = true
                    });
                }
                return Json(new { result = false });
            }
            catch (Exception exc)
            {
                return Json(new { exc.Message });
            }
        }

        // Удаление выбранной задачи.
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
            //var allTasks = context.Tasks.Where(m => m.User.UserData.FirstName.Contains(User.Identity.Name)).ToList();
            var user = context.Users.Where(x => x.UserData.LastName + " " + x.UserData.FirstName + " "
                                                + x.UserData.MiddleName == User.Identity.Name)
                                                .FirstOrDefault();

            var allTasks = context.Tasks.AsParallel().Where(x => x.UserId == user.Id).ToList();

            List<DAL.Task> tasks = new List<DAL.Task>(allTasks);

            return View(tasks);

        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}