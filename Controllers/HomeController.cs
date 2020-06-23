using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsTableForWho.Models;
using Microsoft.AspNetCore.Http;

namespace CsTableForWho.Controllers
{
    public class HomeController : Controller
    {
        private bool isAdmin
        {
            get
            {
                if (HttpContext.Session.GetInt32("currURole") == 1)  //1=Admin
                    return true;
                else
                    return false;
            }
        }
        private dbTableForWhoContext db;
        public HomeController(dbTableForWhoContext dbContext)
        {
            db = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(User newCust)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index");
            }
            db.Users.Add(newCust);
            db.SaveChanges();
            HttpContext.Session.SetInt32("currURole", newCust.RoleId);  //2=Customers
            return RedirectToAction("ShowWaitList");
            // return RedirectToAction("SuccessUser","Home",newCust);
        }

        // public IActionResult ShowWaitList(string value)
        public IActionResult ShowWaitList()
        {
            // if (value=="waiting")
            // else if(value=="seated")
            // else //value=="left"
            List<User> WaitListedCustomers = db.Users   //filtered to Customers only
                    .Where(u => u.RoleId == 2 && u.isSeated == false && u.hasLeft == false)
                    .OrderBy(u => u.CreatedAt)
                    .ToList();

            // if(HttpContext.Session.GetInt32("currURole")==1)    
            if (isAdmin)
            {
                WaitListedCustomers = db.Users      //display all customers and admin users
                    .Where(u => u.RoleId == 2 && u.isSeated == false && u.hasLeft == false)
                    .OrderByDescending(u => u.RoleId)
                    .ThenBy(u => u.CreatedAt)
                    .ToList();
            }

            return View(WaitListedCustomers);
            // return RedirectToAction("SuccessUser","Home",newCust);
        }

        public IActionResult custCheck()
        {
            return RedirectToAction("ShowWaitList");
        }

        public IActionResult ShowAll()  //could I combine this into the ShowWaitList view as well?  -- UseViewBag
        {
            //set viewbag.xxx = "ShowAll"  and then return RedirectToAction("ShowWaitList"); just like all others
            List<User> FullList = db.Users
                    .OrderByDescending(u => u.RoleId)
                    .ThenBy(u => u.CreatedAt)
                    .ToList();
            return View(FullList);
        }

        public IActionResult Seated(int userId)
        {
            User dbCustomer = db.Users
                .FirstOrDefault(u => u.UserId == userId);
            if (dbCustomer != null)
            {
                dbCustomer.isSeated = true;
                dbCustomer.UpdatedAt = DateTime.Now;
            }
            db.Users.Update(dbCustomer);
            db.SaveChanges();
            return RedirectToAction("ShowWaitList");
        }

        public IActionResult Left(int userId)
        {
            User dbCustomer = db.Users
                .FirstOrDefault(u => u.UserId == userId);
            if (dbCustomer != null)
            {
                dbCustomer.hasLeft = true;
                dbCustomer.UpdatedAt = DateTime.Now;
            }
            db.Users.Update(dbCustomer);
            db.SaveChanges();
            return RedirectToAction("ShowWaitList");
        }

        public IActionResult ClearAll()
        //should add a js check - are you sure? before wiping out
        {
            List<User> ClearShift = db.Users.Where(u => u.RoleId == 2).ToList();
            foreach (User u in ClearShift)
            {
                db.Users.Remove(u);
            }
            db.SaveChanges();
            return RedirectToAction("ShowWaitList");
        }






        public IActionResult SuccessUser(User newUser)
        {
            return View(newUser);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
