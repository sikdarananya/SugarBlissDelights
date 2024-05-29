using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_try.Controllers;
using SweetShop_MVC.Models;

namespace SweetShop_MVC.Controllers
{
    
    public class ItemsController : Controller
    {
        private project_tryEntities1 db = new project_tryEntities1();

        // GET: Items
        public ActionResult Index()
        {
            if(AccountController.isUser || AccountController.isLoggedin == false)
                return HttpNotFound();
            return View(db.Items.ToList());
        }

        public ActionResult Index1(string searching)
        {
            if (AccountController.isAdmin || AccountController.isLoggedin == false)
                return HttpNotFound();
            return View(db.Items.Where(x => x.ItemType.Contains(searching) || searching == null).ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult Details1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            if (AccountController.isUser && !AccountController.isLoggedin || AccountController.isLoggedin==false)
                return HttpNotFound();
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ItemName,ItemType,Price,Image, File")] Item item)
        {
            if (AccountController.isUser && !AccountController.isLoggedin || AccountController.isLoggedin == false)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileName(item.File.FileName);
                string _filename = DateTime.Now.ToString("hhmmssfff") + filename;
                string path = Path.Combine(Server.MapPath("/Images/"), _filename);
                item.Image = "/Images/" +  _filename;

                

                db.Items.Add(item);
                try
                {
                    if (item.File.ContentLength < 1000000)
                    {
                        if (db.SaveChanges() > 0)
                        {
                            item.File.SaveAs(path);
                        }
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.msg = "File must be less than or equal to 1MB";
                    }
                }
                catch(DbEntityValidationException e)
                {
                    Console.WriteLine(e);

                }
                
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (AccountController.isUser || AccountController.isLoggedin == false)
                return HttpNotFound();
            if (id == null)
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            Session["imgPath"] = item.Image;
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ItemName,ItemType,Price,Image, File")] Item item)
        {
            if (AccountController.isUser && AccountController.isLoggedin || AccountController.isLoggedin == false)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                if (item.File != null)
                {
                    string filename = Path.GetFileName(item.File.FileName);
                    string _filename = DateTime.Now.ToString("hhmmssfff") + filename;
                    string path = Path.Combine(Server.MapPath("/Images/"), _filename);
                    item.Image = "/Images/" + _filename;



                    

                    if (item.File.ContentLength < 1000000)
                    {
                        db.Entry(item).State = EntityState.Modified;
                        string oldImagePath = Request.MapPath(Session["imgPath"].ToString());
                        if (db.SaveChanges() > 0)
                        {
                            item.File.SaveAs(path);
                            if(System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.msg = "File must be less than or equal to 1MB";
                    }



                }
                else
                {
                    item.Image = Session["imgPath"].ToString();
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (AccountController.isUser || AccountController.isLoggedin == false)
                return HttpNotFound();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.FirstOrDefault(u=>u.id==id);
            var ord = db.orderts.Where(u => u.item_id == item.id);
            db.orderts.RemoveRange(ord);
            string currentImg = Request.MapPath(item.Image);
            db.Items.Remove(item);
            //db.SaveChanges();
            if(db.SaveChanges()>0)
            {
               if(System.IO.File.Exists(currentImg))
                {
                    System.IO.File.Delete(currentImg);
               }
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
