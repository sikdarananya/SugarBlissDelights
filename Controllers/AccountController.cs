using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

using SweetShop_MVC.Models;

namespace Project_try.Controllers
{
    public class AccountController : Controller
    {
        public static bool isLoggedin = false;
        public static bool isAdmin = false;
        public static bool isUser = false;
        project_tryEntities1 db = new project_tryEntities1();
        // GET: Home
        public ActionResult Index()
        {
            if (AccountController.isLoggedin == false || isAdmin == false)
                return HttpNotFound();
            return View(db.TBLUserInfoes.ToList());
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TBLUserInfo tBLUserInfo)
        {
            if (db.TBLUserInfoes.Any(x => x.UsernameUs == tBLUserInfo.UsernameUs))
            {
                ViewBag.Notification = "This account already exists";
                return View();
            }
            else
            {
                db.TBLUserInfoes.Add(tBLUserInfo);
                db.SaveChanges();

                isLoggedin = true;
                Session["Id"] = tBLUserInfo.IdUs.ToString();
                Session["Username"] = tBLUserInfo.UsernameUs.ToString();
                return RedirectToAction("UserLogin", "Account");


            }

        }

        public ActionResult Logout()
        {
            Session.Clear();
            isLoggedin = false;
            isAdmin = false;
            isUser = false;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLUserInfo tBLUserInfo)
        {
            var checkLogin = db.TBLUserInfoes.Where(x => x.UsernameUs.Equals(tBLUserInfo.UsernameUs) && x.Password.Equals(tBLUserInfo.Password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["Id"] = tBLUserInfo.IdUs.ToString();
                Session["Username"] = tBLUserInfo.UsernameUs.ToString();

                if (tBLUserInfo.UsernameUs == "admin" && tBLUserInfo.Password == "admin@123")
                {
                    isLoggedin = true;
                    isAdmin = true;
                    return RedirectToAction("Index", "Items");
                }
                else
                {
                    isLoggedin = false;
                    ViewBag.msg = "Not authorized to login";
                }
            }

            else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }

            return View();

        }

        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult UserLogin(TBLUserInfo tBLUserInfo)
        {
            var checkLogin = db.TBLUserInfoes.Where(x => x.UsernameUs.Equals(tBLUserInfo.UsernameUs) && x.Password.Equals(tBLUserInfo.Password)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["Id"] = tBLUserInfo.IdUs.ToString();
                Session["Username"] = tBLUserInfo.UsernameUs.ToString();
                isLoggedin = true;

                if (tBLUserInfo.UsernameUs == "admin" && tBLUserInfo.Password == "admin@123")
                {
                    isLoggedin = false;
                    ViewBag.msg = "You are admin not authorized to login as user";
                }
                else
                {
                    isLoggedin = true;
                    isUser = true;
                    return RedirectToAction("Index1", "Home");

                }
            }

            else
            {
                ViewBag.Notification = "Wrong Username or Password";
            }

            return View();

        }

        public ActionResult Edit(int id)
        {
            if (isUser == true)
            {
                TBLUserInfo user = db.TBLUserInfoes.Find(id);
                return View(user);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Edit(TBLUserInfo user)
        {
            user.ConfirmPassword = user.Password;
            var order = db.orderts.Where(o => o.cust_id == user.IdUs).ToList();
            user.orderts = order;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            isUser = false;
            isLoggedin = false;
            return RedirectToAction("Editconfirmde");
        }
        public ActionResult Editconfirmde()
        {
            return View();
        }

        public ActionResult Delete(string name)
        {
            if(isUser == true)
            {
                TBLUserInfo del = db.TBLUserInfoes.FirstOrDefault(n => n.UsernameUs == name);
                return View(del);
            }
            else { return HttpNotFound(); }

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(string name)
        {
            TBLUserInfo ln = db.TBLUserInfoes.FirstOrDefault(n => n.UsernameUs == name);
            var order = db.orderts.Where(u => u.cust_id == ln.IdUs);
            db.orderts.RemoveRange(order);
            db.TBLUserInfoes.Remove(ln);
            db.SaveChanges();
            isUser = false;
            isLoggedin = false;
            return RedirectToAction("Index", "Home");

        }





    }
}
