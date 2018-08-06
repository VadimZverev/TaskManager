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
    public class AccauntController : Controller
    {
        private DataContext context = new DataContext();

        // Открытие списка пользователей.
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
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        // Сохранение регистрационных данных.
        [HttpPost]
        public async Task<ActionResult> Register(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<CreateUserViewModel, User>(model);
                UserData userData = new UserData
                {
                    Id = user.Id,
                    FirstName = user.Login
                };
                user.RoleId = 1;

                context.Users.Add(user);
                context.UserDatas.Add(userData);

                await context.SaveChangesAsync();

                return RedirectToAction("ListUser");
            }

            return View(model);
        }

        // Открытие окна редактирования данных пользователя.
        [HttpGet]
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

        [HttpPost]
        public JsonResult EditUserData(UserDataDetailsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = Mapper.Map<UserDataDetailsViewModel, UserData>(model);

                    context.Entry(userData).State = EntityState.Modified;
                    context.SaveChanges();

                    var role = context.Users.Find(model.Id).Role.Name;

                    return Json(new
                    {
                        id = userData.Id,
                        fio = userData.LastName + " " +
                        userData.FirstName + " " +
                        userData.LastName,
                        role,
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


        // При удалении есть конфликт таблиц, надо пересмотреть отношения таблиц.
        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            try
            {
                using (DataContext context = new DataContext())
                {
                    var user = context.Users.Where(x => x.Id == id).FirstOrDefault();
                    //var userData = context.UserDatas.Where(x => x.Id == user.UserDataId).FirstOrDefault();

                    if (user == null)
                    {
                        return Json(new { result = false });
                    }

                    context.Users.Remove(user);
                    //context.UserDatas.Remove(userData);
                    context.SaveChanges();

                    return Json(new { result = true });
                }
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

