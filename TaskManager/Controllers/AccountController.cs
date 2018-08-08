using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManager.DAL;
using TaskManager.Models;
using TaskManager.Providers;

namespace TaskManager.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DataContext context = new DataContext();

        // Открытие окна входа пользователя в систему.
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // Вход пользователя в систему.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.Users.FirstOrDefault(u => u.Login == model.Name && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    HttpContext.Response.Cookies["userDataId"].Value = Convert.ToString(user.UserDataId);
                    Response.Cookies["userDataId"].Expires = DateTime.Now.AddMinutes(2880);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Логин (и)или пароль неверны.");
                }
            }

            return View(model);
        }

        // Выход пользователя из системы.
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Response.Cookies["userDataId"].Value = null;
            return RedirectToAction("Login");
        }

        // Открытие окна регистарции пользователя(Регистрация анонимного пользователя).
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // Сохранение регистрационных данных.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password) == null)
                {
                    context.Users.Add(new User
                    {
                        Login = model.Login,
                        Password = model.Password,
                        RoleId = context.Roles.Where(m => m.Name == "User").FirstOrDefault().Id
                    });
                    context.UserDatas.Add(new UserData
                    {
                        FirstName = model.Login
                    });

                    await context.SaveChangesAsync();
                    FormsAuthentication.SetAuthCookie(model.Login, true);

                    return RedirectToAction("ListProject", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }

        // Открытие списка пользователей.
        [Authorize(Roles = "Administrator")]
        public ActionResult ListUser()
        {
            List<ListUserViewModel> listUser = new List<ListUserViewModel>();
            var users = context.Users.ToList();
            var model = Mapper.Map(users, listUser);
            return View(model);
        }

        // Просмотр информации о пользователе.
        public async Task<ActionResult> UserDataDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var userData = await context.UserDatas.FindAsync(id);
            var model = Mapper.Map<UserData, UserDataDetailsViewModel>(userData);

            return PartialView(model);
        }

        // Создание пользователя (АдминМетод в будущем)
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateUser()
        {
            var role = context.Roles.ToList();
            ViewBag.userRole = new SelectList(role, "Id", "Name");

            return PartialView();
        }

        // сохранение нового пользователя (АдминМетод в будущем)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public JsonResult CreateUser(CreateUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (context.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password) == null)
                    {
                        var userData = Mapper.Map<CreateUserViewModel, UserData>(model);

                        context.Users.Add(new User
                        {
                            Login = model.Login,
                            Password = model.Password,
                            RoleId = model.RoleId
                        });
                        context.UserDatas.Add(userData);
                        context.SaveChanges();

                        return Json(new
                        {
                            id = userData.Id,
                            fio = userData.LastName + " " +
                            userData.FirstName + " " +
                            userData.MiddleName,
                            role = context.Roles.Find(model.RoleId).Name,
                            result = true
                        });
                    }
                    else
                    {
                        return Json(new { result = false, msg = "Пользователь с таким логином уже существует" });
                    }
                }

                return Json(new { result = false });
            }
            catch (Exception exc)
            {

                return Json(new { exc.Message });
            }
        }

        // Открытие окна редактирования данных пользователя.
        public ActionResult EditUserData(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListUser");
            }

            var user = context.Users.Where(m => m.UserDataId == id).FirstOrDefault();
            var userData = context.UserDatas.Find(id);
            

            if (userData != null)
            {
                var roles = context.Roles.ToList();
                Session["userRole"] = new SelectList(roles, "Id", "Name", user.RoleId);

                var model = Mapper.Map<UserData, CreateUserViewModel>(userData);
                model.Login = user.Login;
                model.Password = user.Password;
                model.RoleId = (int)user.RoleId;
                model.UserId = user.Id;

                return PartialView(model);
            }

            return RedirectToAction("ListProject");

        }

        // Сохранение изменений данных пользователя.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditUserData(CreateUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = Mapper.Map<CreateUserViewModel, UserData>(model);
                    var user = Mapper.Map<CreateUserViewModel, User>(model);

                    context.Entry(userData).State = EntityState.Modified;
                    context.Entry(user).State = EntityState.Modified;

                    context.SaveChanges();

                    return Json(new
                    {
                        id = userData.Id,
                        fio = userData.LastName + " " +
                        userData.FirstName + " " +
                        userData.MiddleName,
                        role = context.Roles.Find(model.RoleId).Name,
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

        // Удаление пользователя.
        [HttpPost]
        public async Task<JsonResult> DeleteUserAsync(int id)
        {
            try
            {
                var userData = await context.UserDatas.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (userData == null)
                {
                    return Json(new { result = false });
                }

                var user = await context.Users.Where(x => x.UserDataId == id).FirstOrDefaultAsync();

                if (user.Login.Equals(User.Identity.Name))
                {
                    FormsAuthentication.SignOut();
                }

                context.UserDatas.Remove(userData);
                await context.SaveChangesAsync();

                return Json(new { result = true });
            }
            catch (Exception exc)
            {
                return Json(exc.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}

