using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using _4thtry.Context;

namespace _4thtry.Controllers
{
    public class UserController : Controller
    {
        db_testEntities dbObj = new db_testEntities();
        // GET: User

        //[Authorize(Roles ="admin")]


        public int setuserID()
        {
            int ID;
            string username = User.Identity.Name;
            User db = dbObj.Users.Where(a => a.Name == username).SingleOrDefault();
            ID = db.UserId;
            return ID;

        }
        public ActionResult Index()
        {
            return View();
      
        }
        public ActionResult Signup()
        {
            return View();
        }
    

        [HttpPost]
        public ActionResult AddUsers(User model)
        {
            User obj = new User();
            if (ModelState.IsValid)
            {
                User existinguser = dbObj.Users.Where(x => x.Email == model.Email || x.Name == model.Name).SingleOrDefault();
                if(existinguser != null)
                {
                    ViewBag.Message = "Email or Username already exists";
                }
                else
                {
                    obj.Name = model.Name;
                    obj.Email = model.Email;
                    obj.Password = model.Password;

                    if (model.UserId == 0)
                    {
                        dbObj.Users.Add(obj);
                        dbObj.SaveChanges();
                        ViewBag.Message = "Account Created";
                    }
                    else
                    {
                        dbObj.Entry(obj).State = EntityState.Modified;
                        dbObj.SaveChanges();
                    }
                }
            }
            else
            {
                ViewBag.Message = "Try again";
            }
            ModelState.Clear();
            return View("Signup");

        }
        [Authorize]
        public ActionResult ListUsers()
        {
            var res = dbObj.Users.ToList();
            return View(res);

        }
        public ActionResult Delete(int userid)
        {
            //System.Diagnostics.Debug.WriteLine(userid);
            var res = dbObj.Users.Where(x => x.UserId == userid).First();
            // so it means method Delete isn;t getting any parameter from listusers button code// var res = dbObj.Users.Where(x=> x.UserId==2).First();
            dbObj.Users.Remove(res);
            dbObj.SaveChanges();

            var list = dbObj.Users.ToList();


            return View("ListUsers", list);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User model)
        {
            using (var context = new db_testEntities())
            {
                bool isvalid = dbObj.Users.Any(x => x.Email == model.Email && x.Password == model.Password);
                if (isvalid)
                {
                  //  User db = context.Users.Where(a => a.Email == model.Email && a.Password == model.Password).SingleOrDefault();
                    User db = dbObj.Users.Where(a => a.Email == model.Email && a.Password == model.Password).SingleOrDefault();
                    FormsAuthentication.SetAuthCookie(db.Name, false);
                    return RedirectToAction("ShoppingPage", "Product"); //home page is better
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                    ViewBag.Message="Invalid Login! Try again ";
                }
                /*return RedirectToAction("ShoppingPage", "Product");*/
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
            //< add name = "db_testEntities" connectionString = "metadata=res://*/Context.Model1.csdl|res://*/Context.Model1.ssdl|res://*/Context.Model1.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DESKTOP-4RO7EID;initial catalog=db_test;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName = "System.Data.EntityClient" /></ connectionStrings >
            //<add name="db_testEntities" connectionString="metadata=res://*/Context.Model1.csdl|res://*/Context.Model1.ssdl|res://*/Context.Model1.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=SQL5097.site4now.net,1433;Initial Catalog=db_a82cd4_dbtest;User Id=db_a82cd4_dbtest_admin;Password=ctrl2797;&quot;" providerName="System.Data.EntityClient" /></connectionStrings>
        }
        public ActionResult Animations()
        {
            return View();
        }
    }
}