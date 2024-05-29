using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Project_try.Controllers;
using System.Web.UI.WebControls;
using SweetShop_MVC.Models;
using System.Data.Entity;

namespace SweetShop_MVC.Controllers
{
    public class CartController : Controller
    {
        project_tryEntities1 db = new project_tryEntities1();
        // GET: Cart
        public ActionResult Index()
        {
            if (AccountController.isLoggedin == false)
                return HttpNotFound();
            return View();
        }

        public ActionResult Checkout()
        {
            if (AccountController.isLoggedin == false)
                return HttpNotFound();
            return View();
        }

        public ActionResult Order()
        {
            if (AccountController.isLoggedin == false)
                return HttpNotFound();
            return View();
        }

        public ActionResult Checkout1(string OrderData)
        {
            //if (AccountController.IsAdmin || AccountController.IsLoggedIn == false)
            //    return HttpNotFound();
            var username = Session["Username"];
            var userId = db.TBLUserInfoes.First(u => u.UsernameUs == username).IdUs;
            var orderData = JsonConvert.DeserializeObject<Dictionary<int, int>>(OrderData);
            TBLUserInfo lg = new TBLUserInfo();
            ViewBag.name = username;
            foreach (var item in orderData)
            {
                var order = db.Items.Find(item.Key);
                var price = int.Parse(order.Price);
                var quantity = item.Value;
                var orders = new ordert
                {
                    cust_id = userId,
                    item_id = item.Key,
                    price = price,
                    Quantity = quantity,
                    total_price = price * quantity
                };
                db.orderts.Add(orders);
            }
            db.SaveChanges();
            // Pass the model to the view

            //var ord = db.orderts.Include(p => p.Item);

            return RedirectToAction("Checkout");

        }

        public ActionResult FinalOrders()
        {
            if (AccountController.isUser || AccountController.isLoggedin == false)
               return HttpNotFound();
            var ord = db.orderts.Include(o => o.Item)
                                .Include(u => u.TBLUserInfo);
            return View(ord);
        }
        public ActionResult AddToCart(int ProductID)
        {
            if (Session["cart"] == null)
            {
                List<Items_InCart> cart = new List<Items_InCart>();
                Items_InCart items_InCart = new Items_InCart();
                items_InCart.Product = db.Items.Find(ProductID);
                items_InCart.Quantity = 1;
                cart.Add(items_InCart);
                Session["cart"] = cart;
            }
            else
            {
                List<Items_InCart> cart = (List<Items_InCart>)Session["cart"];
                int index = IsInCart(ProductID);

                if(index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Items_InCart() { Product = db.Items.Find(ProductID), Quantity = 1 });

                }

                Session["cart"] = cart;



            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int ProductID)
        {
            List<Items_InCart> cart = (List<Items_InCart>)Session["cart"];
            int index = IsInCart(ProductID);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");

        }

        public int IsInCart(int ProductID)
        {
            List<Items_InCart> cart = (List<Items_InCart>)Session["cart"];
            for(int i=0; i<cart.Count; i++)
            {
                if (cart[i].Product.id == ProductID)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}