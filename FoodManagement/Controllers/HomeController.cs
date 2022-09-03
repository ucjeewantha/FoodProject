﻿using FoodManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//C:\Users\Aashif Ameer\Source\Repos\FoodProject\FoodManagement\App_Data\
namespace FoodManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        FoodManagementEntities food = new FoodManagementEntities();

        [HttpGet]
        public ActionResult AdminAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminAdd(FOOD_TYPE admin)
        {
            food.FOOD_TYPE.Add(admin);
            food.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult AdminUpdate(int id)
        {
            var data = food.FOOD_TYPE.Where(x => x.TYPEID == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult AdminUpdate(FOOD_TYPE admin, int id)
        {
            var data = food.FOOD_TYPE.FirstOrDefault(x=>x.FOODID==id);
            if (data != null)
            {
                data.NAME = admin.NAME;
                data.PRICE = admin.PRICE;
                data.QUANTITY = admin.QUANTITY;
                food.SaveChanges();
                return RedirectToAction("Display");
            }
            else
            {
                return View();
            }
        }

        public ActionResult AdminDelete(int id)
        {
            var data = food.FOOD_TYPE.Where(x => x.TYPEID == id).FirstOrDefault();
            food.FOOD_TYPE.Remove(data);
            food.SaveChanges();
            return RedirectToAction("Display");
        }

        public ActionResult Display()
        {
            return View(food.FOOD_TYPE.ToList());
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult AdminAuthorize()
        {

            if (Request.Form["user_id"].Equals("F00D123") && Request.Form["password"].Equals("F00DTrain@123"))
            {

                return RedirectToAction("Display");
            }
            else
            {
                ViewBag.msg = "Please provide correct User id and Password";
                return View("AdminLogin");
            }

        }    

        public ActionResult Front()
        {
            return View();
        }
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(USER_REGISTRATION user, string repassword)
        {
            if (ModelState.IsValid && repassword == user.PASSWORD)
            {
                food.USER_REGISTRATION.Add(user);
                food.SaveChanges();
                return RedirectToAction("Login");
            }
            else if(user.PASSWORD != repassword)
            {
                ViewBag.msg = "Password and re-type password are not matched";
                return View(user);
            }
            else
            {
                return View(user);
            }
        }

        // [Authorize]
        public ActionResult Content(string name)
        {
            if (name != null)
            {
                var item = from v in food.FOOD_TYPE
                           where v.NAME.StartsWith(name)
                           select v;
                return View(item);
            }
            else
            {
                return View(food.FOOD_TYPE.ToList());
            }
        }

        //[ValidateAntiForgeryToken]
        public ActionResult Login()
        {

            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult Authenticate()
        {
            if (ModelState.IsValid)
            {
                var data = food.USER_REGISTRATION.ToList();
                int count = 0;

                foreach (USER_REGISTRATION item in data)
                {

                    if (Request.Form["email"].Equals(item.EMAIL) && Request.Form["password"].Equals(item.PASSWORD))
                    {

                        count++;
                    }

                }
                if (count > 0)
                {
                    return RedirectToAction("Content");
                }
                else
                {
                    ViewBag.msg = "Please provide correct E-Mail and Password";
                    return View("Login");
                }

            }
            else
            {
                ViewBag.msg = "Please provide correct E-Mail and Password";
                return View("Login");
            }
        }
    }
}