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
            return View(model);
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
