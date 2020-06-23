using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsTableForWho.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CsTableForWho.Controllers
{
    public class AdminController : Controller
    {
        private dbTableForWhoContext db;
        public AdminController(dbTableForWhoContext dbContext)
        {
            db = dbContext;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoggingIn(LoginUser loggedInAdmin)
        {
            int? dbUid = null;
            int dbRoleId = 0;
            string dbUname = "";

            //initial data entry ok
            if (ModelState.IsValid)
            {
                string failedLoginMsg = "Invalid Login/Password";
                //does login name exist/match?
                User dbUser = db.Users.FirstOrDefault(u => u.Name == loggedInAdmin.LoginName);
                if (dbUser == null)
                {
                    ModelState.AddModelError("LoginName", failedLoginMsg);
                }
                else
                {
                    //does password match?
                    dbUid = dbUser.UserId;
                    dbRoleId = dbUser.RoleId;
                    dbUname = dbUser.Name;
                    PasswordHasher<LoginUser> pHasher = new PasswordHasher<LoginUser>();
                    PasswordVerificationResult compResult = pHasher.VerifyHashedPassword(loggedInAdmin, dbUser.Password, loggedInAdmin.LoginPassword);
                    if (compResult == 0)  //0 = Failed
                    {
                        ModelState.AddModelError("LoginName", failedLoginMsg);
                    }
                }
            }
            if (ModelState.IsValid == false)
            {
                return View("Login");
            }
            HttpContext.Session.SetInt32("currUid", (int)dbUid);
            HttpContext.Session.SetInt32("currURole", dbRoleId);
            HttpContext.Session.SetString("currUname", dbUname);
            return RedirectToAction("ShowWaitList", "Home");
            // return View("SuccessLogin",loggedInAdmin);
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Registering(User newAdmin)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Name == newAdmin.Name))
                {
                    ModelState.AddModelError("Name", "...already exists.");
                }
            }
            if (ModelState.IsValid == false)
            {
                return View("Register");
            }

            //registering entry ok - continue
            PasswordHasher<User> pHasher = new PasswordHasher<User>();
            newAdmin.Password = pHasher.HashPassword(newAdmin, newAdmin.Password);
            db.Users.Add(newAdmin);
            db.SaveChanges();
            HttpContext.Session.SetInt32("currUid", newAdmin.UserId);
            HttpContext.Session.SetInt32("currURole", newAdmin.RoleId);  //1=Admin
            HttpContext.Session.SetString("currUname", newAdmin.Name);
            return RedirectToAction("ShowWaitList", "Home");
            // return RedirectToAction("SuccessUser","Home",newAdmin);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
