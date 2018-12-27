using RentalSystem.Helper;
using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace RentalSystem.Controllers.Customer
{
    public class CustomerController : Controller
    {

        // GET: /Customer/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            bool loginFailed = Convert.ToBoolean(TempData["CustomerLoginFailed"]);
            bool registrationFailed = Convert.ToBoolean(TempData["CustomerRegistrationFailed"]);

            ViewBag.Login = false;
            ViewBag.Register = false;
            ViewBag.IsPostBack = false;
            if (loginFailed)
            {
                ViewBag.Login = true;
            }

            if (registrationFailed)
            {
                ViewBag.IsPostBack = true;
            }
            return View();
        }

        //
        // POST: /Vendor/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            UserLoginViewModel user = null;
            try
            {
                if (ModelState.IsValid)
                {
                    user = new UserLoginViewModel { Password = model.Password, Email = model.Email, RoleId = (int)Role.Customer };
                    user = await UserManager.CreateUser(user);
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }

            ViewBag.Login = false;
            ViewBag.Register = false;
            ViewBag.IsPostBack = false;

            if (user != null)
            {
                ViewBag.IsPostBack = true;
                ViewBag.Register = true;
                return View();
            }

            TempData["CustomerRegistrationFailed"] = true;
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register");
        }

        // POST: /Customer/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(RegisterViewModel model)
        {
            bool status = false;
            UserLoginViewModel user = null;
            try
            {
                user = new UserLoginViewModel { Password = model.Password, Email = model.Email, RoleId = (int)Role.Customer };
                user = await UserManager.Login(user);
                ViewBag.Login = false;
                ViewBag.Register = false;
                if (user.Id != 0)
                {
                    Session["CustomerLogIn"] = true;
                    Session["CustomerImage"] = user.Password.ToString();
                    Session["CustomerLoginId"] = user.Id.ToString();
                    Session["CustomerEmail"] = user.Email;
                    Session["CustomerId"] = user.RoleId.ToString();
                    ViewBag.Login = true;
                    return RedirectToAction("UpdateCustomer");
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            TempData["CustomerLoginFailed"] = true;
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register");
        }

        //GET: Logout Customer
        public ActionResult LogOut()
        {
            Session["CustomerLogIn"] = null;
            Session["CustomerImage"] = null;
            Session["CustomerLoginId"] = null;
            Session["CustomerEmail"] = null;
            Session["CustomerId"] = null;
            return RedirectToAction("Register");
        }

        //GET:Customer/Update
        [HttpGet]
        public ActionResult UpdateCustomer()
        {
            ViewBag.Status = false;
            ViewBag.Error = false;
            return View();
        }


        //POST:Customer/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCustomer(UserUploadViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("UpdateCustomer");
            }
            UserViewModel user = null;
            try
            {
                user = new UserViewModel
                {
                    Id = model.Id,
                    Email = model.Email,
                    Name = model.Name,
                    Contact = model.Contact,
                    Age = model.Age,
                    PaymentId = model.PaymentId,
                    Address = model.Address,
                    Valid = true
                };
                if (model.Photo != null)
                    user.Photo = SaveImage(model.Photo);
                else
                    user.Photo = "";
                user = ApiHelper.Add<UserViewModel>(user, URL.LocalIISURL, "update");
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }

            if (user != null)
            {
                if (!String.IsNullOrEmpty(user.Photo))
                {
                    Session["CustomerImage"] = user.Photo.ToString();
                }
                ViewBag.Error = false;
                ViewBag.Status = true;
                return View();
            }
            ViewBag.Error = true;
            ViewBag.Status = false;
            return View();
        }
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Approve
        public ActionResult Approve()
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        private string SaveImage(HttpPostedFileBase file)
        {
            string fName = "";
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                fileName = Guid.NewGuid() + fileName + extension;
                fName = fileName;
                fileName = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(fileName);
            }
            catch (Exception e)
            {

                Log.Error("Image Not Updated: Customer Controller: SaveImage ", e);
            }
            return fName;
        }

        private void ExceptionLogging(Exception e)
        {
            string actionName = "";
            string controllerName = "";
            try
            {
                actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Log.Fatal("Controller :" + controllerName + ", Action :" + actionName, e);
            }
            catch (Exception ex)
            {

                Log.Warn("Exception in ExceptionLogging Method inside Customer Controller", ex);
            }
        }

    }
}
