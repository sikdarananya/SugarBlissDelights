using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetShop_MVC.Models
{
    public class Items_InCart
    {
        public Item Product { get; set; }

        public int Quantity { get; set; }
    }
}