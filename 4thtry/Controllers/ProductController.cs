using System;
using _4thtry.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4thtry.Context;
using System.Net;
using System.Data.Entity;

namespace _4thtry.Controllers
{
    //add this for add restriction unless logged in [Authorize]

    public class ProductController : Controller
    {
        db_testEntities dbObj = new db_testEntities();

        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
            User dbuser = new User();

            int u_id = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            List<Cart> li = dbObj.Carts.Where(x => x.user_id_fk == u_id).ToList();
            TempData["cart"] = li;
            List<Cart> li2 = TempData["cart"] as List<Cart>;


            if (TempData["cart"] != null)
            {
                double x = 0;
                foreach (var item in li2)
                {
                    x += item.total;
                }
                TempData["total"] = x;
            }
            TempData.Keep();
            //return View(dbObj.products.OrderByDescending(x => x.product_id).ToList());



            return View(dbObj.products.ToList());


            //return View(dbObj.products.OrderByDescending(x=> x.product_id).ToList());
        }
        public ActionResult Product()
        {
            return View(dbObj.products.ToList());
        }

        public int returnUserID()
        {
            int ID;
            string username = User.Identity.Name;
            User db = dbObj.Users.Where(a => a.Name == username).SingleOrDefault();
            ID = db.UserId;
            return ID;
        }


        public ActionResult ShoppingPage(string searchstring)
        {
            if (searchstring == null)
            {
                return View(dbObj.products.ToList());
            }
            else
            {
                return View(dbObj.products.Where(x => x.product_name.Contains(searchstring)).ToList());
            }


        }


        [Authorize(Roles = "admin")]
        public ActionResult AddnewProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddnewProduct(HttpPostedFileBase file, product pro_model)
        {
            product obj = new product();
            //img ko lagi
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            obj.product_img = "~/images/" + _filename;

            //text type ko lagi

            obj.product_name = pro_model.product_name;
            obj.product_price = pro_model.product_price;
            obj.product_stock = pro_model.product_stock;
            obj.product_des = pro_model.product_des;
            //no need //obj.product_img = obj.product_img;
            //obj.product_img = pro_model.product_img;
            //check ntra tala ko ma obj change //dbObj.products.Add(obj);



            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
            {
                if (file.ContentLength <= 2 * 1024 * 1024)
                {

                    dbObj.products.Add(obj);
                    if (dbObj.SaveChanges() > 0)
                    {
                        file.SaveAs(path);

                        //dbObj.SaveChanges();
                        ModelState.Clear();

                    }
                }


            }
            else
            {
                ViewBag.msg = "File size exceeded";
            }

            return View();
        }
        /*public ActionResult AddProduct(product pro_model)
        {
            product obj = new product();
            obj.product_name = pro_model.product_name;
            obj.product_price = pro_model.product_price;
            obj.product_stock = pro_model.product_stock;
            obj.product_des = pro_model.product_des;
            obj.product_img = pro_model.product_img;
            dbObj.products.Add(obj);
            dbObj.SaveChanges();

            return View(obj);
        }

        [HttpPost]*/
        /*public ActionResult addtocart(int? pro_id)
        {
            product p = dbObj.products.Where(x => x.product_id == pro_id).SingleOrDefault();
            return View(p);
        }*/
        List<Cart> li = new List<Cart>();

        //[HttpPost]
        /*public ActionResult addtocart(*//*product pi,*//*string qty,*//*int pro_id)*/


        [Authorize(Roles = "admin,customer")]
        public ActionResult addtocart(int? id, string qty)
        {
            product p = (dbObj.products.Where(x => x.product_id == id).SingleOrDefault());

            Cart c = new Cart();

            //if (var p = (db.tbl_product.Where(x => x.pro_id == Id).SingleOrDefault()))

            if (qty == null)
            {
                qty = "1";
            }
            c.user_id_fk = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            c.pro_id_fk = p.product_id;
            c.qty = Convert.ToInt32(qty);
            c.unitprice = p.product_price;
            c.total = c.qty * c.unitprice;

            bool isalready = dbObj.Carts.Any(x => x.pro_id_fk == id && x.user_id_fk == c.user_id_fk);
            if (isalready)
            {
                //add to existing row
                Cart existedcart1 = dbObj.Carts.Where(x => x.pro_id_fk == id && x.user_id_fk == c.user_id_fk).SingleOrDefault();
                existedcart1.user_id_fk = c.user_id_fk;
                existedcart1.pro_id_fk = c.pro_id_fk;
                existedcart1.qty = existedcart1.qty + c.qty;
                existedcart1.unitprice = c.unitprice;
                existedcart1.total = existedcart1.qty * existedcart1.unitprice;
                dbObj.Entry(existedcart1).State = EntityState.Modified;
                dbObj.SaveChanges();
                TempData.Keep();
            }
            else
            {
                dbObj.Carts.Add(c);
                dbObj.SaveChanges();
                TempData.Keep();
            }



            /*List<Cart1> listofcart2 = db.Cart1.Where(x => x.pro_id_fk == Id && x.user_id_fk == c.pro_id_fk).ToList();

            int flag = 0;
            foreach (var item in listofcart2)
            {
                if (item.pro_id_fk == c.pro_id_fk)
                {
                    c.qty = item.qty + c.qty;
                    c.total = item.total + c.total;
                    flag = 1;
                }
            }
            if (flag == 0)
            {
                db.Cart1.Add(c);
            }


            db.Entry(c).State = EntityState.Modified;*/
            /*db.Cart1.Add(c);
            db.SaveChanges();
            TempData.Keep();*/






            return RedirectToAction("ShoppingPage");

            /*product p = dbObj.products.Where(x => x.product_id == id).SingleOrDefault();
            string qty = "1";
            Cart c = new Cart();
            c.product_id = p.product_id;
            c.price = (float)p.product_price;
            c.qty = Convert.ToInt32(qty);
            c.bill = c.price * c.qty;
            c.product_name = p.product_name;
            c.product_img = p.product_img;
            if(TempData["cart"]== null)
            {
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.product_id == c.product_id)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }
                TempData["cart"] = li2;
            }
            TempData.Keep();
            return RedirectToAction("ShoppingPage");*/

        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tbl_product
            var tbl_img = dbObj.products.Find(id);

            Session["imgPath"] = tbl_img.product_img;

            if (tbl_img == null)
            {
                return HttpNotFound();
            }
            return View(tbl_img);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(HttpPostedFileBase file, product emp)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/images/"), _filename);
                    emp.product_img = "~/images/" + _filename;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 1000000)
                        {
                            dbObj.Entry(emp).State = EntityState.Modified;
                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());
                            if (dbObj.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);


                                }

                                TempData["msg"] = "Record updated";
                                //db.Entry(emp).State = EntityState.Modified;
                                //ViewBag.msg = "Record Added";
                                //ModelState.Clear();
                            }
                        }
                        else
                        {
                            ViewBag.msg = "Size is not valid";
                        }
                    }
                }
            }
            else
            {
                emp.product_img = Session["imgPath"].ToString();
                dbObj.Entry(emp).State = EntityState.Modified;


                if (dbObj.SaveChanges() > 0)
                {
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");
                }
            }



            return View();

        }


        public ActionResult CartPage()
        {

            TempData.Keep();
            return View();


        }
        [Authorize(Roles = "customer,admin")]
        public ActionResult addqtya(int? id)
        {
            Cart carttoedit = dbObj.Carts.Where(x => x.cart_id == id).SingleOrDefault();
            carttoedit.qty = carttoedit.qty + 1;
            carttoedit.total = carttoedit.unitprice * carttoedit.qty;
            dbObj.Entry(carttoedit).State = EntityState.Modified;
            dbObj.SaveChanges();
            int u_id = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            List<Cart> li = dbObj.Carts.Where(x => x.user_id_fk == u_id).ToList();
            TempData["cart"] = li;

            TempData.Keep();
            return RedirectToAction("checkout");
        }
        [Authorize(Roles = "customer,admin")]
        public ActionResult subqty(int? id)
        {



            Cart carttoedit = dbObj.Carts.Where(x => x.cart_id == id).SingleOrDefault();
            if (carttoedit.qty == 1)
            {
                dbObj.Carts.Remove(carttoedit);
                dbObj.SaveChanges();
            }
            else
            {
                carttoedit.qty = carttoedit.qty - 1;
                carttoedit.total = carttoedit.unitprice * carttoedit.qty;
                dbObj.Entry(carttoedit).State = EntityState.Modified;
                dbObj.SaveChanges();
            }

            int u_id = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            List<Cart> li = dbObj.Carts.Where(x => x.user_id_fk == u_id).ToList();
            TempData["cart"] = li;

            TempData.Keep();
            return RedirectToAction("checkout");
        }

        [Authorize(Roles = "customer,admin")]
        public ActionResult checkout()
        {
            int u_id = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            List<Cart> li = dbObj.Carts.Where(x => x.user_id_fk == u_id).ToList();
            TempData["cart"] = li;
            //List<Cart> li2 = TempData["cart"] as List<Cart>;
            if (li.Count != 0)
            {
                double x = 0;
                foreach (var item in li)
                {
                    x += item.total;
                }
                TempData["total"] = x;
            }
            else
            {
                TempData["cart"] = null;
            }

            TempData.Keep();
            //tbl_product product = db.tbl_product.Where(x => x.pro_id = li.);
            //List<tbl_product> listofproduct = db.Cart1.Where(x => x.pro_id_fk == ).ToList();
            return View();


            /*TempData.Keep();
            return View();*/
        }
        [HttpPost]
        public ActionResult checkout(order o, string address, string phone_no)
        {

            int id = returnUserID();
            List<Cart> licheckout = dbObj.Carts.Where(x => x.user_id_fk == id).ToList();
            bill iv = new bill();
            iv.invoice_fk_user = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            iv.invoice_date = System.DateTime.Now;
            //iv.in_totalbill = (float)TempData["total"];
            iv.invoice_totalbill = float.Parse(TempData["total"].ToString());
            dbObj.bills.Add(iv);
            dbObj.SaveChanges();
            foreach (var item in licheckout)
            {

                order od = new order();
                od.order_fkpro = item.pro_id_fk;
                od.order_fk_invoice = iv.invoice_id;
                od.order_date = System.DateTime.Now;
                od.order_qty = item.qty;
                od.order_status = "Processing";
                var selectproduct = dbObj.Carts.Where(x => x.pro_id_fk == item.pro_id_fk).SingleOrDefault();
                //od.o_unitprice = selectproduct.pro_price;
                od.order_unitprice = item.unitprice;
                od.order_bill = od.order_unitprice * od.order_qty;
                od.address = address;
                od.phone_no = phone_no;
                if (address == "Nayabazar")
                {
                    od.assigned_emp_id = 1;
                }

                else if (address == "Newroad")
                {
                    od.assigned_emp_id = 2;
                }
                else if (address == "Mahendrapool")
                {
                    od.assigned_emp_id = 3;
                }
                else if (address == "Prithvi Chowk")
                {
                    od.assigned_emp_id = 4;
                }
                else if (address == "Lakeside")
                {
                    od.assigned_emp_id = 5;
                }
                else if (address == "Amarsing Chowk")
                {
                    od.assigned_emp_id = 6;
                }

                dbObj.orders.Add(od);
                dbObj.Carts.Remove(item);
                dbObj.SaveChanges();
            }
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData["msg"] = "transaction completed";

            TempData.Keep();
            return RedirectToAction("ShoppingPage");



            /*if (TempData["cart"] != null)
            {
                float x = 0;
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                foreach (var item in li2)
                {
                    x += item.bill;
                }
                TempData["total"] = x;
            }
            TempData.Keep();




            List<Cart> li = TempData["cart"] as List<Cart>;
            bill iv = new bill();
            iv.invoice_fk_user = Convert.ToInt32(Session["u_id"].ToString());
            iv.invoice_date = System.DateTime.Now;
            iv.invoice_totalbill = (float)TempData["total"];
            dbObj.bills.Add(iv);
            dbObj.SaveChanges();
            foreach (var item in li)
            {
                order od = new order();
                od.order_fkpro = item.pro_id_fk;
                od.order_fk_invoice = iv.invoice_id;
                od.order_date = System.DateTime.Now;
                od.order_qty = item.qty;

                od.order_unitprice = (int)item.price;
                od.order_bill = item.bill;
                dbObj.orders.Add(od);
                dbObj.SaveChanges();


            }
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData["msg"] = "transaction completed";

            TempData.Keep();
            return RedirectToAction("Product");*/
        }

        public ActionResult remove(int? id)
        {
            /*List<Cart> li2 = TempData["cart"] as List<Cart>;
            Cart c = li2.Where(x => x.cart_id == id).SingleOrDefault();
            dbObj.Carts.Remove(c);
            li2.Remove(c);
            double h = 0;
            foreach (var item in li2)
            {
                h += item.total;
                
                dbObj.SaveChanges();
            }
            //TempData["total"] = h;
            return RedirectToAction("cartpage");*/
            List<Cart> li2 = TempData["cart"] as List<Cart>;
            Cart carttoberemoved = (dbObj.Carts.Where(x => x.cart_id == id).SingleOrDefault());
            Cart c1 = li2.Where(x => x.cart_id == id).SingleOrDefault();
            dbObj.Carts.Remove(carttoberemoved);
            li2.Remove(c1);
            dbObj.SaveChanges();

            double h = 0;
            foreach (var item in li2)
            {
                h += item.total;
            }
            TempData["total"] = h;
            TempData.Keep();
            return RedirectToAction("checkout");

        }
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var productToBeRemoved = dbObj.products.Find(id);
            product productToBeRemoved = dbObj.products.Where(x => x.product_id == id).SingleOrDefault();
            if (productToBeRemoved == null)
            {

            }

            string currentimg = Request.MapPath(productToBeRemoved.product_img);
            dbObj.products.Remove(productToBeRemoved);
            //dbObj.Entry(productToBeRemoved).State = EntityState.Deleted;
            if (dbObj.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(currentimg))
                {
                    System.IO.File.Delete(currentimg);
                }
                TempData["msg"] = "Succesfully Deleted";
            }
            //
            //product newdb = dbObj.products.Where(a => a.u_name == model.u_name && a.u_password == model.u_password).SingleOrDefault();
            //dbObj.products.Remove(tobeRemovedProduct);


            return RedirectToAction("Product");
        }
        [Authorize(Roles = "admin,customer")]
        public ActionResult ViewMyOrders()
        {
            int id = returnUserID();
            //Convert.ToInt32(Session["u_id"].ToString());
            List<order> listofmyorder = dbObj.orders.Where(x => x.bill.invoice_fk_user == id).ToList();
            return View(listofmyorder);
        }
        [Authorize(Roles ="admin")]
        public ActionResult ViewAllOrders()
        {
            List<order> listofallorders = dbObj.orders.ToList();
            return View(listofallorders);
        }
        /*[HttpPost]
        public ActionResult ViewAllOrders(int? id)
        {
            order ordertobeEdited = dbObj.orders.Where(x => x.order_id == id).SingleOrDefault();
            return View();
        }*/
        [Authorize(Roles ="admin")]
        public ActionResult EditOrders(int? id)
        {
            order ordertobeEdited = dbObj.orders.Where(x => x.order_id == id).SingleOrDefault();
            return View(ordertobeEdited);
        }
        [Authorize(Roles ="admin")]
        [HttpPost]
        public ActionResult EditOrders(int? id,order modelorder)
        {
            string ordstatus = modelorder.order_status.ToString();
            id = modelorder.order_id;
            order ordertobeEdited = dbObj.orders.Where(x => x.order_id == id).SingleOrDefault();
            ordertobeEdited.order_status = ordstatus;
            dbObj.Entry(ordertobeEdited).State = EntityState.Modified;
            dbObj.SaveChanges();
            if (ordstatus == "Delivered")
            {
                bill orderbill = dbObj.bills.Where(x => x.invoice_id == ordertobeEdited.order_fk_invoice).SingleOrDefault();
                orderbill.Ispaid = "Yes";



                product prodStock = dbObj.products.Where(x => x.product_id == ordertobeEdited.order_fkpro).SingleOrDefault();
                prodStock.product_stock = prodStock.product_stock - (int)ordertobeEdited.order_qty;
                dbObj.Entry(prodStock).State = EntityState.Modified;
                dbObj.Entry(orderbill).State = EntityState.Modified;
                dbObj.SaveChanges();
            }


            return View();
        }
        public ActionResult cancelOrder(int? id)
        {
            List<order> ordertoberemoved = dbObj.orders.Where(x => x.order_id == id).ToList();
            if (ordertoberemoved.Count == 1)
            {
                
                foreach(var item in ordertoberemoved)
                {
                    bill billtoberemoved = dbObj.bills.Where(x => x.invoice_id == item.order_fk_invoice).SingleOrDefault();
                    dbObj.orders.Remove(item);
                    dbObj.bills.Remove(billtoberemoved);
                    dbObj.SaveChanges();
                }

            }
            else
            {
                foreach (var item in ordertoberemoved)
                {
                    bill billtobeadjusted = dbObj.bills.Where(x => x.invoice_id == item.order_fk_invoice).SingleOrDefault();

                    double amounttobereduced = Convert.ToInt32(item.order_qty) * (double)item.order_unitprice;
                    billtobeadjusted.invoice_totalbill = billtobeadjusted.invoice_totalbill - amounttobereduced;
                    dbObj.Entry(billtobeadjusted).State = EntityState.Modified;
                    dbObj.orders.Remove(item);
                    dbObj.SaveChanges();
                }
            }
            /*foreach(var item in ordertoberemoved)
            {
                bill billtoberemoved = dbObj.bills.Where(x => x.invoice_id == item.order_fk_invoice).SingleOrDefault();
                dbObj.orders.Remove(item);
                *//*dbObj.SaveChanges();*//*
                
                dbObj.bills.Remove(billtoberemoved);
                dbObj.SaveChanges();
            }*/
            return RedirectToAction("checkout");
        }
        
        public ActionResult ProductPage(int? id)
        {
            product pi = dbObj.products.Where(x => x.product_id == id).SingleOrDefault();
            TempData.Keep();
            return View(pi);
        }
        [HttpPost]
        public ActionResult ProductPage(int? id, string qty)
        {
            //List<product> p = dbObj.products.ToList();
            
            product pi = dbObj.products.Where(x => x.product_id == id).SingleOrDefault();
            TempData.Keep();
            return View(pi);
        }
        [Authorize]
        public ActionResult ProductPagewithreview(int? id)
        {
            List<reply> liReplies = dbObj.replies.ToList();
            TempData["replies"] = liReplies;
            product pi = dbObj.products.Where(x => x.product_id == id).SingleOrDefault();
            TempData.Keep();
            return View(pi);
        }
        [HttpPost]
        public ActionResult ProductPagewithreview(int? id, string qty)
        {

            product pi = dbObj.products.Where(x => x.product_id == id).SingleOrDefault();
            TempData.Keep();
            return View(pi);
        }

        public ActionResult AddReview(string msg, int productID,int? ratings, int? isreplytoid)
        {
            int id = returnUserID();
                //Convert.ToInt32(Session["u_id"].ToString());
            if (isreplytoid!=null)
                //if it is a reply it will add to the rply table
            {
                reply replytobeadded = new reply();
                replytobeadded.fk_reviewId = (int)isreplytoid;
                replytobeadded.msg = msg;
                replytobeadded.user_id = id;
                replytobeadded.postdate = System.DateTime.Now;
                dbObj.replies.Add(replytobeadded);
                dbObj.SaveChanges();
                review reviewtobemodified = dbObj.reviews.Where(x => x.review_id == isreplytoid).SingleOrDefault();
                reviewtobemodified.isthere_reply = "yes";
                dbObj.Entry(reviewtobemodified).State = EntityState.Modified;
                dbObj.SaveChanges();
                
            }
            else
            //if it is a review it will add to the review table
            {
                
                review reviewtobeadded = new review();
                reviewtobeadded.product_id = productID;
                reviewtobeadded.user_id = id;
                reviewtobeadded.review_msg = msg;
                reviewtobeadded.Postdate = System.DateTime.Now;
                if (ratings != null)
                {
                    reviewtobeadded.ratings = ratings;
                }
                dbObj.reviews.Add(reviewtobeadded);
                dbObj.SaveChanges();
            }
            TempData.Keep();
            return RedirectToAction("checkcanreview", new { id = id });
        }
        public ActionResult checkcanreview(int? id)
        {
            int uid = returnUserID();
                //Convert.ToInt32(Session["u_id"].ToString());
            bool ordercheck = dbObj.orders.Any(x => x.order_fkpro == id && x.bill.invoice_fk_user == uid && x.order_status == "Delivered");

            List<review> li = dbObj.reviews.Where(x => x.product_id == id).ToList();
            TempData["listofreview"] = li;
            if (ordercheck)
            {
                TempData.Keep();
                return RedirectToAction("ProductPagewithreview", new { id = id });
            }
            else
            {
                TempData.Keep();
                return RedirectToAction("ProductPage", new { id = id });
            }
                
        }
            
        }
    }
