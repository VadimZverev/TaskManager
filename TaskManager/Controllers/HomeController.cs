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
        // Тестовый метод поиска проектов через поисковик.
        //[HttpPost]
        //public ActionResult ProjectSearch(string name)
        //{
        //    List<ListProjectViewModel> listProjectViewModel = new List<ListProjectViewModel>();
        //    var allProject = context.Projects.Where(m => m.User.UserData.FirstName.Contains(name)).ToList();
        //    var model = Mapper.Map(allProject, listProjectViewModel);

        //    return PartialView(model);
        //}

        // Открытие списка проектов
        [Authorize]
        public ActionResult ListProject()
        {
            IEnumerable<Project> projects;
            IEnumerable<ListProjectViewModel> model;

            using (DataContext context = new DataContext())
            {
                projects = context.Projects.ToList();
                model = Mapper.Map(projects, new List<ListProjectViewModel>());
            }

            return View(model);
        }

        // Открытие окна создания проекта
        public ActionResult CreateProject()
        {
            List<SelectListItem> items;
            List<SelectListItem> projectManager = new List<SelectListItem>();

            using (DataContext context = new DataContext())
            {

                items = context.Users
                    .Where(m => m.Role.Name.Contains("Project Manager"))
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.UserData.LastName + " " + p.UserData.FirstName + " " + p.UserData.MiddleName
                    }).ToList();

                projectManager.AddRange(items);
            }

            ViewBag.ProjectManager = projectManager;

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
                    Project project;
                    User user;

                    using (DataContext context = new DataContext())
                    {
                        project = Mapper.Map<CreateProjectViewModel, Project>(model);
                        user = context.Users.Find(model.UserId);
                        context.Projects.Add(project);
                        context.SaveChanges();

                        return Json(new
                        {
                            ProjectId = project.Id,
                            ProjectName = model.Name,
                            ProjectManager = user.UserData.LastName + " " +
                                        user.UserData.FirstName + " " +
                                        user.UserData.MiddleName,
                            DateCreate = project.DateCreate.ToShortDateString(),
                            result = true
                        });
                    }
                }

                return Json(new { result = false });
            }
            catch (Exception exc)
            {
                return Json(new { msg = exc.Message });
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

            List<SelectListItem> items;
            List<SelectListItem> projectManager = new List<SelectListItem>();

            using (DataContext context = new DataContext())
            {
                var project = context.Projects.Find(id);

                if (project != null)
                {
                    items = context.Users
                        .Where(m => m.Role.Name.Contains("Project Manager"))
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.UserData.LastName + " " + p.UserData.FirstName + " " + p.UserData.MiddleName
                        }).ToList();

                    projectManager.AddRange(items);

                    ViewBag.ProjectManager = projectManager;

                    var projectEdit = Mapper.Map<Project, EditProjectViewModel>(project);

                    if (projectEdit.DateClose != null)
                    {
                        projectEdit.ProjectClose = true;
                    }
                    return PartialView(projectEdit);
                }
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
                    using (DataContext context = new DataContext())
                    {
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
                using (DataContext context = new DataContext())
                {
                    var project = context.Projects.Where(x => x.Id == id).FirstOrDefault();

                    if (project == null)
                    {
                        return Json(new { result = false });
                    }

                    context.Projects.Remove(project);
                    context.SaveChanges();
                }
                return Json(new { result = true });
            }
            catch (Exception exc)
            {
                return Json(new { msg = exc.Message });
            }
        }

        // Список задач по проекту.
        public ActionResult ListTask(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            using (DataContext context = new DataContext())
            {
                var tasks = context.Tasks.Where(m => m.ProjectId == id).ToList();
                var model = Mapper.Map(tasks, new List<ListTaskViewModel>());

                var projectName = context.Projects.FirstOrDefault(m => m.Id == id);
                ViewBag.Project = projectName.Name;
                ViewBag.ProjectId = projectName.Id;

                return PartialView(model);
            }
        }

        // Открытие окна создания задачи.
        [HttpGet]
        public async Task<ActionResult> CreateTask(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListProject");
            }

            using (DataContext context = new DataContext())
            {
                ViewBag.ProjectId = id;

                var types = await context.TaskTypes.ToListAsync();
                ViewBag.TaskTypes = new SelectList(types, "Id", "Name");

                var priorities = await context.TaskPriorities.ToListAsync();
                ViewBag.TaskPriorities = new SelectList(priorities, "Id", "Name");


                var users = await context.Users.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.UserData.LastName + " "
                        + p.UserData.FirstName + " "
                        + p.UserData.MiddleName
                }).ToListAsync();

                ViewBag.TaskUser = new List<SelectListItem>(users);

                var statuses = await context.TaskStatuses.ToListAsync();
                ViewBag.TaskStatuses = new SelectList(statuses, "Id", "Name");
            }
            return PartialView();
        }

        // сохранение созданной задачи.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateTask(CreateTaskViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DataContext context = new DataContext())
                    {
                        var task = Mapper.Map<CreateTaskViewModel, DAL.Task>(model);
                        var user = context.Users.Find(model.UserId);

                        context.Tasks.Add(task);
                        context.SaveChanges();

                        return Json(new
                        {
                            taskId = task.Id,
                            taskName = model.TaskName,
                            taskType = context.TaskTypes.AsParallel().Where(x => x.Id == model.TaskTypeId).FirstOrDefault().Name,
                            description = model.Description,
                            taskPriority = context.TaskPriorities.AsParallel().Where(x => x.Id == model.TaskPriorityId).FirstOrDefault().Name,
                            taskUser = $"{user.UserData.LastName} {user.UserData.FirstName} {user.UserData.MiddleName}",
                            taskStatus = context.TaskStatuses.AsParallel().Where(x => x.Id == model.TaskStatusId).FirstOrDefault().Name,
                            DateCreate = task.DateCreate.ToShortDateString(),
                            result = true
                        });
                    }
                }

                return Json(new { result = false });
            }
            catch (Exception exc)
            {
                return Json(new { exc.InnerException.InnerException.Message });
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

            using (DataContext context = new DataContext())
            {
                var task = await context.Tasks.FindAsync(id);

                if (task != null)
                {
                    var types = await context.TaskTypes.ToListAsync();
                    ViewBag.TaskTypes = new SelectList(types, "Id", "Name", task.TaskTypeId);

                    var priorities = await context.TaskPriorities.ToListAsync();
                    ViewBag.TaskPriorities = new SelectList(priorities, "Id", "Name", task.TaskPriorityId);

                    var users = await context.Users.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.UserData.LastName + " "
                            + p.UserData.FirstName + " "
                            + p.UserData.MiddleName
                    }).ToListAsync();

                    ViewBag.TaskUser = new List<SelectListItem>(users);

                    var statuses = await context.TaskStatuses.ToListAsync();
                    ViewBag.TaskStatuses = new SelectList(statuses, "Id", "Name", task.TaskStatusId);

                    var taskEdit = Mapper.Map<DAL.Task, EditTaskViewModel>(task);

                    if (taskEdit.DateClose != null)
                    {
                        taskEdit.TaskClose = true;
                    }

                    return PartialView(taskEdit);
                }

                return RedirectToAction("ListTask", new { id = task.ProjectId });
            }
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

                    using (DataContext context = new DataContext())
                    {
                        context.Entry(task).State = EntityState.Modified;
                        await context.SaveChangesAsync();

                        var user = await context.Users.FindAsync(model.UserId);

                        return Json(new
                        {
                            taskId = task.Id,
                            taskName = model.TaskName,
                            taskType = context.TaskTypes.AsParallel().Where(x => x.Id == model.TaskTypeId).FirstOrDefault().Name,
                            description = model.Description,
                            taskPriority = context.TaskPriorities.AsParallel().Where(x => x.Id == model.TaskPriorityId).FirstOrDefault().Name,
                            taskUser = $"{user.UserData.LastName} {user.UserData.FirstName} {user.UserData.MiddleName}",
                            taskStatus = context.TaskStatuses.AsParallel().Where(x => x.Id == model.TaskStatusId).FirstOrDefault().Name,
                            DateCreate = model.DateCreate.ToShortDateString(),
                            DateClose = model.DateClose.HasValue ? model.DateClose.Value.ToShortDateString() : "",
                            result = true
                        });
                    }
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
                using (DataContext context = new DataContext())
                {
                    var task = context.Tasks.Where(x => x.Id == id).FirstOrDefault();

                    if (task == null)
                    {
                        return Json(new { result = false });
                    }

                    context.Tasks.Remove(task);
                    context.SaveChanges();
                }


                return Json(new { result = true });
            }
            catch (Exception exc)
            {
                return Json(exc.Message);
            }
        }

        public ActionResult Index()
        {
            User user;
            List<DAL.Task> allTasks;
            IEnumerable<UserTasksViewModel> model;
            using (DataContext context = new DataContext())
            {
                user = context.Users.Where(x => x.UserData.LastName + " " + x.UserData.FirstName + " "
                                                    + x.UserData.MiddleName == User.Identity.Name)
                                                    .FirstOrDefault();

                allTasks = context.Tasks.AsParallel().Where(x => x.UserId == user.Id).ToList();

                model = Mapper.Map(allTasks, new List<UserTasksViewModel>());
            }

            return View(model);
        }
    }
}