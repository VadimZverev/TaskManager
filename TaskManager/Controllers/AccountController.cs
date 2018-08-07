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

namespace TaskManager.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private DataContext context = new DataContext();

        // Открытие окна входа пользователя в систему.
        public ActionResult Login()
        {
            return View();
        }

        // Вход пользователя в систему.
        [HttpPost]
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
        public ActionResult Register()
        {
            return View();
        }

        // Сохранение регистрационных данных.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password) == null)
                {
                    //var roleId = context.Roles.Where(m=> m.Name == "User").FirstOrDefault().Id;
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

            List<UserDataDetailsViewModel> UserDetails = new List<UserDataDetailsViewModel>();
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

            return View();
        }

        // Открытие окна редактирования данных пользователя.

        [Authorize]
        public ActionResult EditUserData(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ListUser");
            }

            List<UserDataDetailsViewModel> UserDetails = new List<UserDataDetailsViewModel>();
            var userData = context.UserDatas.Find(id);

            if (userData != null)
            {
                var model = Mapper.Map<UserData, UserDataDetailsViewModel>(userData);
                return PartialView(model);
            }

            return RedirectToAction("ListProject");

        }

        // Сохранение изменений данных пользователя.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult EditUserData(UserDataDetailsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = Mapper.Map<UserDataDetailsViewModel, UserData>(model);

                    context.Entry(userData).State = EntityState.Modified;
                    context.SaveChanges();

                    return Json(new
                    {
                        id = userData.Id,
                        fio = userData.LastName + " " +
                        userData.FirstName + " " +
                        userData.MiddleName,
                        role = context.Users.Find(model.Id).Role.Name,
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
        [Authorize]
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

