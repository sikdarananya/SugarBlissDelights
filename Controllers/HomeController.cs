using Project_try.Controllers;
using SweetShop_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace SweetShop_MVC.Controllers
{
    
    public class HomeController : Controller
    {
        
        private project_tryEntities1 db = new project_tryEntities1();

        //[Authorize]
        public ActionResult Index()
        {
                return View(db.Items.ToList());
        }
       public ActionResult Index1()
        {
            if (AccountController.isAdmin == true || AccountController.isLoggedin==false)
                return HttpNotFound();
            var username = Session["Username"];
            var userid = db.TBLUserInfoes.First(u => u.UsernameUs == username).IdUs;
            ViewBag.userid = userid;
            ViewBag.name = username;


            return View(db.Items.ToList());
        }

        public ActionResult AboutPage()
        {
            return View();
        }

        public ActionResult ContactPage()
        {
            return View();
        }

       

        
    }
}