using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _4thtry.Models
{
    public class cart
    {
        public int product_id { get; set; }
        public string product_img { get; set; }
        public string product_name { get; set; }
        public float price { get; set; }
        public int qty { get; set; }
        public float bill { get; set; }
    }
}