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

        public async Task<ActionResult> ListUser()
        {
            List<ListUserViewModel> listUser = new List<ListUserViewModel>();
            var users = await context.Users.ToListAsync();
            var model = Mapper.Map(users, listUser);
            return View(model);
        }

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

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserViewModel model)
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

        [HttpGet]
        public async Task<ActionResult> EditUserData(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            List<UserDataDetailsViewModel> UserDetails = new List<UserDataDetailsViewModel>();
            var userData = await context.UserDatas.FindAsync(id);

            if (userData != null)
            {
                var model = Mapper.Map<UserData, UserDataDetailsViewModel>(userData);
                return PartialView(model);
            }

            return RedirectToAction("ListProject");

        }

        [HttpPost]
        public async Task<ActionResult> EditUserData(UserDataDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userData = Mapper.Map<UserDataDetailsViewModel, UserData>(model);

                context.Entry(userData).State = EntityState.Modified;

                await context.SaveChangesAsync();
                var id = userData.Id;
                return RedirectToAction("UserDataDetails", new { id = userData.Id });
            }

            return View(model);

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

