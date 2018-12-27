using RentalSystem.Helper;
using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace RentalSystem.Controllers.Vendor
{
    [Route("Vendor/{action?}/{id?}")]
    public class VendorController : Controller
    {


        //GET:Vendor/Update
        [HttpGet]
        public ActionResult UpdateVendor()
        {
            ViewBag.Status = false;
            ViewBag.Error = false;
            return View();
        }


        //POST:Vendor/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateVendor(UserUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("UpdateVendor");
            }
            UserViewModel user = null;
            try
            {
                user = UpdateVendorDetails(model);
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            if (user != null)
            {
                if (!String.IsNullOrEmpty(user.Photo))
                {
                    Session["VendorImage"] = user.Photo.ToString();
                }
                ViewBag.Error = false;
                ViewBag.Status = true;
                return View();
            }
            ViewBag.Error = true;
            ViewBag.Status = false;
            return View();
        }

        //Update Vendor Details
        private UserViewModel UpdateVendorDetails(UserUploadViewModel model)
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





        // GET: /Vendor/Register
        public ActionResult Register()
        {
            bool loginFailed = Convert.ToBoolean(TempData["loginFailed"]);
            bool registrationFailed = Convert.ToBoolean(TempData["registrationFailed"]);

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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            UserLoginViewModel user = null;
            if (ModelState.IsValid)
            {
                try
                {
                    user = new UserLoginViewModel { Password = model.Password, Email = model.Email, RoleId = (int)Role.Vendor };
                    user = await UserManager.CreateUser(user);
                }
                catch (Exception e)
                {

                    ExceptionLogging(e);
                }
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

            TempData["registrationFailed"] = true;
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register");
        }

        // POST: /Vendor/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(RegisterViewModel model)
        {
            UserLoginViewModel user = null;
            try
            {
                user = new UserLoginViewModel { Password = model.Password, Email = model.Email, RoleId = (int)Role.Vendor };
                user = await UserManager.Login(user);
                ViewBag.Login = false;
                ViewBag.Register = false;
                if (user.Id != 0)
                {
                    Session["VendorLogIn"] = true;
                    Session["VendorImage"] = user.Password.ToString();
                    Session["VendorEmail"] = user.Email;
                    Session["VendorId"] = user.RoleId.ToString();
                    Session["VendorLoginId"] = user.Id.ToString();
                    ViewBag.Login = true;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            TempData["loginFailed"] = true;
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register");
        }

        //GET: Logout Vendor
        public ActionResult LogOut()
        {
            Session["VendorLogIn"] = null;
            Session["VendorImage"] = null;
            Session["VendorLoginId"] = null;
            Session["VendorEmail"] = null;
            Session["VendorId"] = null;
            return RedirectToAction("Register");
        }

        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }


        // GET: Vendor/Edit/5
        public ActionResult Edit(int id = 0)
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

        // POST: Vendor/Edit/5
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

        // GET: Vendor/Update/5
        public ActionResult Update(int id = 0)
        {
            ViewBag.Status = false;

            ViewBag.Categories = GetCategories();
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            ProductViewModel product = new ProductViewModel();
            AddProductViewModel prod = new AddProductViewModel();
            try
            {
                ViewBag.Status = Convert.ToBoolean(TempData["ProductUpdateFailed"]);
                product = ApiHelper.GetFromApi<ProductViewModel>(product, URL.LocalIISURL, "products/" + id);
                if (product != null)
                {
                    prod.Id = product.Id;
                    prod.VendorId = product.VendorId;
                    prod.Name = product.Name;
                    prod.Image1 = product.Image1;
                    prod.Image2 = product.Image2;
                    prod.Image3 = product.Image3;
                    prod.Availability = product.Availability;
                    prod.Description = product.Description;
                    prod.StartDate = product.StartDate;
                    prod.EndDate = product.EndDate;
                    prod.CategoryId = product.CategoryId;
                    prod.UnitPrice = product.UnitPrice;
                }
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            return View(prod);
        }

        //POST Vendor/Update(product)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AddProductViewModel addProduct)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Update");
            }
            ProductViewModel product = null;
            try
            {
                product = SaveImages(addProduct);
                if (product == null)
                {
                    return RedirectToAction("AddProduct");
                }
                product.Availability = true;
                int vendorId = 0;
                if (Session["VendorId"] != null)
                {
                    int.TryParse(Session["VendorId"].ToString(), out vendorId);
                }
                product.VendorId = vendorId;
                product = ApiHelper.Add<ProductViewModel>(product, URL.LocalIISURL, "products/update");
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            if (product != null)
            {
                return RedirectToAction("Index");
            }
            TempData["ProductUpdateFailed"] = true;
            return RedirectToAction("Update");
        }

        //GET Vendor/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.Status = false;
            ViewBag.Categories = GetCategories();
            return View();
        }


        //POST Vendor/AddProduct(product)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(AddProductViewModel addProduct)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddProduct");
            }

            ProductViewModel product = null;
            try
            {
                product = SaveImages(addProduct);
                if (product == null)
                {
                    return RedirectToAction("AddProduct");
                }
                product.Availability = true;
                int vendorId = 0;
                if (Session["VendorId"] != null)
                {
                    int.TryParse(Session["VendorId"].ToString(), out vendorId);
                }
                product.VendorId = vendorId;

                product = ApiHelper.Add<ProductViewModel>(product, URL.LocalIISURL, "products");
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            if (product != null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Status = true;
            ViewBag.Categories = GetCategories();
            return View();
        }

        //GET : All products
        [HttpGet]
        public ActionResult AllProducts()
        {

            return View();
        }

        //GET : All products on rent
        [HttpGet]
        public ActionResult AllRent()
        {
            int vendorId = Convert.ToInt32(Session["VendorId"]);
            ViewBag.VendorId = vendorId;
            if (vendorId == 0)
                return RedirectToAction("Index");
            return View();
        }

        //GET : Approve view
        [HttpGet]
        public ActionResult Approve()
        {
            int vendorId = Convert.ToInt32(Session["VendorId"]);
            ViewBag.VendorId = vendorId;
            if (vendorId == 0)
                return RedirectToAction("Index");
            return View();
        }
        #region SaveImages, GetCategories,Log Exceptions
        // return ProductModel object
        private ProductViewModel SaveImages(AddProductViewModel addProduct)
        {
            ProductViewModel productViewModel = null;
            bool imagesSavedSuccessfully = true;
            try
            {
                if (addProduct.UploadImage1 != null)
                {
                    //Save Image 1
                    var file = addProduct.UploadImage1;
                    addProduct.Image1 = SaveImage(file);
                }
                if (addProduct.UploadImage2 != null)
                {
                    //Save Image 2
                    var file = addProduct.UploadImage2;
                    addProduct.Image2 = SaveImage(file);
                }
                if (addProduct.UploadImage3 != null)
                {
                    //Save Image 3
                    var file = addProduct.UploadImage3;
                    addProduct.Image3 = SaveImage(file);
                }
            }
            catch (Exception e)
            {
                imagesSavedSuccessfully = false;
                ExceptionLogging(e);
            }
            if (imagesSavedSuccessfully)
            {
                productViewModel = UpdateProductViewModel(addProduct);
            }
            return productViewModel;

        }

        private static ProductViewModel UpdateProductViewModel(AddProductViewModel addProduct)
        {
            return new ProductViewModel()
            {
                Id = addProduct.Id,
                Name = addProduct.Name,
                Description = addProduct.Description,
                Image1 = addProduct.Image1,
                Image2 = addProduct.Image2,
                Image3 = addProduct.Image3,
                StartDate = addProduct.StartDate,
                EndDate = addProduct.EndDate,
                CategoryId = addProduct.CategoryId,
                UnitPrice = addProduct.UnitPrice
            };
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

                throw e;
            }

            return fName;
        }

        //GET: Get all Categories
        private IEnumerable<CategoryViewModel> GetCategories()
        {
            IEnumerable<CategoryViewModel> list = null;
            try
            {
                list = ApiHelper.GetDataFromApi<CategoryViewModel>(URL.LocalIISURL, "products/GetAllCategories");
            }
            catch (Exception e)
            {
                ExceptionLogging(e);
            }
            return list;

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
        #endregion

        // GET: Vendor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
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

    }
}
