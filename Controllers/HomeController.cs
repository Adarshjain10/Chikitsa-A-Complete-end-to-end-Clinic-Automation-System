using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClinicalAutomationSystem.Controllers
{
    public class HomeController : Controller
    {
        static int count = 0;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel dt)
        {
            if(ModelState.IsValid)
            {
                CASDatabaseEntities db = new CASDatabaseEntities();

                var getData = db.Users.FirstOrDefault(m => m.UserName == dt.UserName);
                if(getData != null)
                {
                    if (getData.IsLocked == false)
                    {
                        if (getData.Password != dt.Password)
                        {
                            count++;
                            if (count == 1)
                            {
                                ViewBag.text = "Login Failed. Invalid Password. Two More Attempts Left.";
                            }
                            else if (count == 2)
                            {
                                ViewBag.text = "Login Failed. Invalid Password. One More Attempt Left.";
                            }
                            else if (count == 3)
                            {
                                getData.IsLocked = true;
                                db.SaveChanges();
                                ViewBag.text = "Login Failed. Invalid Password. No More Attempts Left.";
                            }
                        }
                        else
                        {
                            getData.IsActive = true;
                            getData.LastLogDate = DateTime.Today;
                            getData.IsEmailVerified = true;
                            db.SaveChanges();

                            var getRole = db.RoleDetails.Where(m => m.RoleID == getData.RoleID).Select(a => new { a.RoleName }).FirstOrDefault();
                            FormsAuthentication.SetAuthCookie(dt.EmailID, false);

                            var authTicket = new FormsAuthenticationTicket(1,
                                getData.UserName,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(20),
                                false,
                                getRole.RoleName);

                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            HttpContext.Response.Cookies.Add(authCookie);

                            Session["EmailID"] = getData.EmailID;
                            Session["UserID"] = getData.UserID;
                            Session["UserName"] = getData.UserName;
                            Session["RoleName"] = getRole.RoleName;

                            switch (getRole.RoleName)
                            {
                                case "Admin":
                                    return RedirectToAction("Index", "Admin");
                                case "Physician":
                                    return RedirectToAction("Index", "Physician");
                                case "Patient":
                                    return RedirectToAction("Index", "Patient");
                                case "Supplier":
                                    return RedirectToAction("Index", "Supplier");
                                case "Salesperson":
                                    return RedirectToAction("Index", "Salesperson");
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ViewBag.text = "Account is Locked. Contact Administrator";
                    }
                }
                else
                {
                    ViewBag.text = "Invalid Username.";
                }
            }
            else
            {
                ViewBag.text = "Please Enter Credentials";
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            ModelState.Clear();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RequestAdmin()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SelectListItem> list = new List<SelectListItem>();
            RequestDataModel dt = new RequestDataModel();

            var getData = db.RoleDetails.ToList();

            foreach (var item in getData)
            {
                if (item.RoleID == 1 || item.RoleID == 4)
                {
                    continue;
                }
                else
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleID.ToString()
                    });
                }
            }
            dt.ListRole = list;
            return View(dt);
        }

        [HttpPost]
        public ActionResult RequestAdmin(RequestDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            RequestAdmin objRequest = new RequestAdmin();

            List<SelectListItem> list = new List<SelectListItem>();
            var getData = db.RoleDetails.ToList();

            foreach (var item in getData)
            {
                if (item.RoleID == 1 || item.RoleID == 4)
                {
                    continue;
                }
                else
                {
                    list.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleID.ToString()
                    });
                }
            }
            dt.ListRole = list;

            if(ModelState.IsValid)
            {
                var getEmail = db.Users.Where(m => m.EmailID == dt.EmailID).FirstOrDefault();
                if(getEmail != null)
                {
                    ViewBag.text = "This E-mail ID is already registered.";
                }
                else
                {
                    objRequest.FirstName = dt.FirstName;
                    objRequest.LastName = dt.LastName;
                    objRequest.EmailID = dt.EmailID;
                    objRequest.RoleID = dt.RoleID;
                    objRequest.Status = "Requested";

                    db.RequestAdmins.Add(objRequest);
                    db.SaveChanges();

                    ViewBag.text = "Request has been sent. Please wait for the E-mail.";
                }
            }
            return View(dt);
        }

        public ActionResult Drugs()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<DrugDataModel> list = new List<DrugDataModel>();
            DrugDataModel dt = new DrugDataModel();

            var getDetails = from d in db.Drugs
                             where d.IsDeleted == false
                             select new
                             {
                                 d.DrugName,
                                 d.Manufacturer,
                                 d.Substitutions,
                                 d.Uses,
                                 d.SideEffects,
                                 d.NotRecommended,
                                 d.MfgDate,
                                 d.ExpDate,
                                 d.QOH,
                                 d.QOHType,
                                 d.Price,
                                 d.DiscountAmount,
                             };
            foreach (var item in getDetails)
            {
                list.Add(new DrugDataModel
                {
                    DrugName = item.DrugName,
                    Manufacturer = item.Manufacturer,
                    Substitutions = item.Substitutions,
                    Uses = item.Uses,
                    SideEffects = item.SideEffects,
                    NotRecommended = item.NotRecommended,
                    MfgDate = item.MfgDate,
                    ExpDate = item.ExpDate,
                    QOH = item.QOH,
                    QOHType = item.QOHType,
                    Price = Convert.ToDouble(item.Price),
                    DiscountAmount = Convert.ToDouble(item.DiscountAmount),
                });
            }
            dt.ListDrugs = list;
            return View(dt);
        }

        public ActionResult Physician()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<PhysicianDataModel> list = new List<PhysicianDataModel>();
            PhysicianDataModel dt = new PhysicianDataModel();

            var getDetails = (from p in db.Physicians
                             join s in db.SpecializationDatas
                             on p.SpecializationID equals s.SpecializationID
                             select new
                             {
                                 p.FirstName,
                                 p.LastName,
                                 p.Gender,
                                 p.TotalExperience,
                                 p.Education,
                                 p.CurrentStatus,
                                 s.SpecializationName
                             }).OrderBy(m => m.SpecializationName);

            foreach (var item in getDetails)
            {
                list.Add(new PhysicianDataModel
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    TotalExperience = item.TotalExperience,
                    Education = item.Education,
                    CurrentStatus = item.CurrentStatus,
                    SpecializationName = item.SpecializationName
                });
            }
            dt.ListPhysician = list;
            return View(dt);
        }
    }
}