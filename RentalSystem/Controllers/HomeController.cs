using RentalSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace RentalSystem.Controllers
{
    public class HomeController : Controller
    {
        //GET: Get all Avaialable Products
        public ActionResult Index()
        {
            IEnumerable<ProductViewModel> list = null;
            try
            {
                list = ApiHelper.GetDataFromApi<ProductViewModel>(URL.LocalIISURL, "products/available");
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            return View(list.ToList());
        }

        //Get Details of Product using productId
        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            string email = null;
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            ProductViewModel product = new ProductViewModel();
            try
            {
                product = ApiHelper.GetFromApi<ProductViewModel>(product, URL.LocalIISURL, "products/" + id);
                if (Session["CustomerEmail"] != null)
                {
                    email = Session["CustomerEmail"].ToString();
                    ViewBag.Email = email;
                }
                else
                {
                    ViewBag.Email = "";
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            return View(product);
        }

        //Logout all active user
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
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

                Log.Warn("Exception in ExceptionLogging Method inside Vendor Controller", ex);
            }
        }
    }
}