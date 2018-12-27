using log4net;
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

namespace RentalSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: AllCustomers
        public ActionResult AllCustomers()
        {

            return View();
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Id = id;
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            return View();
        }

        // GET: Admin/Login
        public ActionResult Login()
        {
            bool Status = false;
            ViewBag.Status = false;
            try
            {
                if (TempData["adminLoginFailed"] != null)
                {
                    Status = (bool)TempData["adminLoginFailed"];
                    ViewBag.Status = Status;
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }

            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            bool status = false;
            UserLoginViewModel user = null;
            try
            {
                if (ModelState.IsValid)
                {
                    user = new UserLoginViewModel { Password = model.Password, Email = model.Email, RoleId = (int)Role.Admin };
                    user = await UserManager.Login(user);
                }
                ViewBag.Login = false;
                ViewBag.Register = false;
                if (user != null && user.Id != 0)
                {
                    Session["AdminLogIn"] = true;
                    Session["AdminImage"] = user.Password.ToString();
                    Session["AdminLoginId"] = user.Id.ToString();
                    Session["AdminEmail"] = user.Email;
                    Session["AdminId"] = user.RoleId.ToString();
                    ViewBag.Login = true;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            TempData["adminLoginFailed"] = true;
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Login");
        }


        // GET: Admin
        public ActionResult Vendor(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            UserViewModel user = new UserViewModel();
            try
            {
                user = ApiHelper.GetFromApi<UserViewModel>(user, URL.LocalIISURL, "users/" + id);
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            if (user == null)
            {
                user = new UserViewModel();
            }
            return View(user);
        }
        public ActionResult ProductDetails(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            ProductViewModel product = new ProductViewModel();
            try
            {
                product = ApiHelper.GetFromApi<ProductViewModel>(product, URL.LocalIISURL, "products/" + id);
                UserLoginViewModel user = new UserLoginViewModel();
                user = ApiHelper.GetFromApi<UserLoginViewModel>(user, URL.LocalIISURL, "users/" + product.VendorId);
                ViewBag.Email = user.Email;
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            return View(product);
        }



        //GET:Vendor/Update
        [HttpGet]
        public ActionResult Update()
        {
            ViewBag.Status = false;
            ViewBag.Error = false;
            return View();
        }

        //GET: Logout Customer
        public ActionResult LogOut()
        {
            Session["AdminLogIn"] = null;
            Session["AdminImage"] = null;
            Session["AdminLoginId"] = null;
            Session["AdminEmail"] = null;
            Session["AdminId"] = null;
            return RedirectToAction("Login");
        }


        //POST:Vendor/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Update");
            }
            UserViewModel user = null;
            try
            {
                user = UpdateAdmin(model);
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            bool flag = false;
            if (user != null)
            {
                if (!String.IsNullOrEmpty(user.Photo))
                {
                    Session["AdminImage"] = user.Photo.ToString();
                }
                ViewBag.Error = false;
                ViewBag.Status = true;
                return View();
            }
            ViewBag.Error = true;
            ViewBag.Status = false;
            return View();
        }


        #region SaveImage
        //Save Image file
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

                Log.Error("Image Not Updated:Admin Controller: SaveImage ", e);
            }

            return fName;
        }
        //Update Admin Details
        private UserViewModel UpdateAdmin(UserUploadViewModel model)
        {
            UserViewModel user = new UserViewModel
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
            return user;
        }
        //Log Exceptions
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

                Log.Warn("Exception in ExceptionLogging Method inside Admin Controller", ex);
            }
        }
        #endregion

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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

        // GET: Admin/Edit/id
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/id
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


    }


}
