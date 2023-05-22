using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            var id = Convert.ToInt32(Session["UserID"]);
            var checkName = db.Admins.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.AdminID;
            return View();
        }

        [Authorize]
        public ActionResult EditDetails()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            AdminDataModel dt = new AdminDataModel();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Admins.Where(a => a.UserID == id).FirstOrDefault();

            if(getData != null)
            {
                dt.FirstName = getData.FirstName;
                dt.LastName = getData.LastName;
                dt.Gender = getData.Gender;
                dt.Address = getData.Address;
            }
            else
            {
                dt.FirstName = null;
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditDetails(AdminDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Admin objAdmin = new Admin();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Admins.Where(a => a.UserID == id).FirstOrDefault();
            if(ModelState.IsValid)
            {
                if(getData != null)
                {
                    getData.FirstName = dt.FirstName;
                    getData.LastName = dt.LastName;
                    getData.Gender = dt.Gender;
                    getData.Address = dt.Address;
                }
                else
                {
                    objAdmin.FirstName = dt.FirstName;
                    objAdmin.LastName = dt.LastName;
                    objAdmin.Gender = dt.Gender;
                    objAdmin.Address = dt.Address;

                    db.Admins.Add(objAdmin);
                }
                db.SaveChanges();
            }
            var checkName = db.Admins.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.AdminID;
            return View(dt);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            User objUser = new User();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Admins.Where(a => a.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if(getData != null)
                {
                    var getInfo = (from u in db.Users
                                   where (u.UserID == id)
                                   select new
                                   {
                                       u.EmailID,
                                       u.Password,
                                   }).FirstOrDefault();
                    if(dt.OldPassword == dt.NewPassword)
                    {
                        ViewBag.text = "New Password Cannot Be Same As Old Password";
                    }
                    else
                    {
                        if(getInfo.Password == dt.OldPassword)
                        {
                            var getEmail = db.Users.FirstOrDefault(m => m.EmailID == getInfo.EmailID);

                            if (getEmail != null)
                            {
                                getEmail.Password = dt.NewPassword;
                                db.SaveChanges();
                                ViewBag.text = "Password Updated Successfully.";
                            }
                        }
                        else
                        {
                            ViewBag.text = "Password Incorrect. Please Enter Correct Password.";
                        }
                    }
                }
            }
            return View(dt);
        }

        [Authorize]
        public ActionResult Register()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SelectListItem> list = new List<SelectListItem>();
            DataViewModel dt = new DataViewModel();

            var getData = db.RoleDetails.ToList();

            foreach (var item in getData)
            {
                if (item.RoleID == 1)
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
        public ActionResult Register(DataViewModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            User objUser = new User();
            Random random = new Random();

            List<SelectListItem> list = new List<SelectListItem>();
            var getData = db.RoleDetails.ToList();

            foreach (var item in getData)
            {
                if (item.RoleID == 1)
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

            if (ModelState.IsValid)
            {
                var getUserName = db.Users.Where(m => m.UserName == dt.UserName).FirstOrDefault();
                if(getUserName != null)
                {
                    ViewBag.text = "User Name is taken. Please choose another.";
                }
                else
                {
                    long pass = random.Next();
                    objUser.UserName = dt.UserName;
                    objUser.EmailID = dt.EmailID;
                    objUser.Password = pass.ToString();
                    objUser.RoleID = dt.RoleID;
                    objUser.IsActive = false;
                    objUser.IsLocked = false;
                    objUser.IsEmailVerified = false;

                    db.Users.Add(objUser);
                    db.SaveChanges();

                    SendVerificationLink(dt.EmailID, dt.UserName, pass);
                    Session["Register"] = "Registered";

                    var getUserID = db.Users.Where(m => m.RoleID == dt.RoleID).OrderByDescending(o => o.UserID).FirstOrDefault();
                    dt.UserID = getUserID.UserID;
                    switch (dt.RoleID)
                    {
                        case 2:
                            Patient pa = new Patient();
                            pa.UserID = getUserID.UserID;
                            db.Patients.Add(pa);
                            db.SaveChanges();
                            break;                           
                        case 3:
                            Physician ph = new Physician();
                            ph.UserID = getUserID.UserID;
                            db.Physicians.Add(ph);
                            db.SaveChanges();
                            break;
                        case 4:
                            Salesperson sa = new Salesperson();
                            sa.UserID = getUserID.UserID;
                            db.Salespersons.Add(sa);
                            db.SaveChanges();
                            break;
                        case 5:
                            Supplier s = new Supplier();
                            s.UserID = getUserID.UserID;
                            db.Suppliers.Add(s);
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }

                    ViewBag.text = "Registered Successfully. Account Activation Link Sent.";
                }         
            }
            else
            {
                ViewBag.text = "Registration Unsuccessful.";
            }

            return View(dt);
        }

        [NonAction]
        public void SendVerificationLink(string EmailID, string Username, long Password)
        {
            var verifyUrl = "/Home/Login/";
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("cas.for.ibm@gmail.com", "clini-C-are | Clinical Automation System");
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = "ibm150520";
            string subject = "Your account is successfully created!";
            string body = "<br/> <br/> We are excited to inform you that your account at clini-C-are has been created." +
                "Use the same to track your activities. Please click on the below link to verify you account and LOGIN!" +
                "<br/> <b> UserName: <b> " + Username + "<br/> <b> Password: <b>" + Password +
                "<br/> <br/> <a href = '" + link + "'> </a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

            smtp.Send(message);
        }

        [Authorize]
        public ActionResult ViewRequests()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<RequestDataModel> list = new List<RequestDataModel>();
            RequestDataModel dt = new RequestDataModel();

            var getData = from ra in db.RequestAdmins
                          join r in db.RoleDetails
                          on ra.RoleID equals r.RoleID
                          where ra.Status == "Requested"
                          select new
                          {
                              ra.RequestID,
                              ra.FirstName,
                              ra.LastName,
                              ra.EmailID,
                              ra.Status,
                              r.RoleName
                          };

            foreach(var item in getData)
            {
                list.Add(new RequestDataModel
                {
                    RequestID = item.RequestID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    EmailID = item.EmailID,
                    Status = item.Status,
                    RoleName = item.RoleName,
                });
            }
            dt.ListRequest = list;

            return View(dt);
        }

        public ActionResult OnRequest(int id, string str)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();

            var getData = db.RequestAdmins.Where(m => m.RequestID == id).FirstOrDefault();

            if (str == "Accepted")
            {
                getData.Status = "Accepted";
                db.SaveChanges();
                return RedirectToAction("Register", "Admin");
            }
            
            
            db.RequestAdmins.Remove(getData);
            db.SaveChanges();
            return RedirectToAction("ViewRequests", "Admin");
        }

        [Authorize]
        public ActionResult ViewPatient()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<PatientDataModel> list = new List<PatientDataModel>();
            PatientDataModel dt = new PatientDataModel();

            var getData = from p in db.Patients
                          join u in db.Users
                          on p.UserID equals u.UserID
                          where p.FirstName != null
                          select new
                          {
                              p.PatientID,
                              p.FirstName,
                              p.LastName,
                              p.DOB,
                              p.Gender,
                              p.Address,
                              p.ContactNo,
                              p.EmgContactName,
                              p.EmgContactNo,
                              u.EmailID
                          };
            foreach (var item in getData)
            {
                list.Add(new PatientDataModel
                {
                    PatientID = item.PatientID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    DOB = Convert.ToDateTime(item.DOB),
                    Gender = item.Gender,
                    Address = item.Address,
                    ContactNo = item.ContactNo,
                    EmgContactName = item.EmgContactName,
                    EmgContactNo = item.EmgContactNo,
                    EmailID = item.EmailID,
                });
            }
            dt.ListPatient = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewPhysician()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<PhysicianDataModel> list = new List<PhysicianDataModel>();
            PhysicianDataModel dt = new PhysicianDataModel();

            var getData = from p in db.Physicians
                          join s in db.SpecializationDatas
                          on p.SpecializationID equals s.SpecializationID
                          join u in db.Users
                          on p.UserID equals u.UserID
                          where p.FirstName != null
                          select new
                          {
                              p.PhysicianID,
                              p.FirstName,
                              p.LastName,
                              p.Gender,
                              p.TotalExperience,
                              p.Education,
                              p.CurrentStatus,
                              s.SpecializationName,
                              u.EmailID
                          };
            foreach (var item in getData)
            {
                list.Add(new PhysicianDataModel
                {
                    PhysicianID = item.PhysicianID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    TotalExperience = item.TotalExperience,
                    Education = item.Education,
                    CurrentStatus = item.CurrentStatus,
                    SpecializationName = item.SpecializationName,
                    EmailID = item.EmailID,
                }); 
            }
            dt.ListPhysician = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewSalesperson()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SalesDataModel> list = new List<SalesDataModel>();
            SalesDataModel dt = new SalesDataModel();

            var getData = from s in db.Salespersons
                          join u in db.Users
                          on s.UserID equals u.UserID
                          where s.FirstName != null
                          select new
                          {
                              s.SalespersonID,
                              s.FirstName,
                              s.LastName,
                              s.CurrentStatus,
                              u.EmailID
                          };
            foreach (var item in getData)
            {
                list.Add(new SalesDataModel
                {
                    SalespersonID = item.SalespersonID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    CurrentStatus = item.CurrentStatus,
                    EmailID = item.EmailID,
                });
            }
            dt.ListSalesperson = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewSupplier()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SupplierDataModel> list = new List<SupplierDataModel>();
            SupplierDataModel dt = new SupplierDataModel();

            var getData = from s in db.Suppliers
                          join u in db.Users
                          on s.UserID equals u.UserID
                          where s.FirstName != null
                          select new
                          {
                              s.SupplierID,
                              s.FirstName,
                              s.LastName,
                              s.CompanyName,
                              s.CompanyAddress,
                              s.CurrentStatus,
                              u.EmailID
                          };
            foreach (var item in getData)
            {
                list.Add(new SupplierDataModel
                {
                    SupplierID = item.SupplierID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    CompanyName = item.CompanyName,
                    CompanyAddress = item.CompanyAddress,
                    CurrentStatus = item.CurrentStatus,
                    EmailID = item.EmailID,
                });
            }
            dt.ListSupplier = list;
            return View(dt);
        }

        public ActionResult DeleteUser(int id, string str)
        {
            switch(str)
            {
                case "Patient":
                    CASDatabaseEntities db = new CASDatabaseEntities();
                    var getPatient = db.Patients.Where(m => m.PatientID == id).FirstOrDefault();
                    var getPUser = db.Users.Where(m => m.UserID == getPatient.UserID).FirstOrDefault();
                    if(getPUser != null)
                    {
                        getPUser.IsLocked = true;
                    }
                    db.SaveChanges();
                    return RedirectToAction("ViewPatient", "Admin");

                case "Physician":
                    CASDatabaseEntities dbp = new CASDatabaseEntities();
                    var getPhysician = dbp.Physicians.Where(m => m.PhysicianID == id).FirstOrDefault();
                    var getPhUser = dbp.Users.Where(m => m.UserID == getPhysician.UserID).FirstOrDefault();
                    if (getPhUser != null)
                    {
                        getPhUser.IsLocked = true;
                    }
                    dbp.SaveChanges();
                    return RedirectToAction("ViewPhysician", "Admin");

                case "Salesperson":
                    CASDatabaseEntities dbs = new CASDatabaseEntities();
                    var getSales = dbs.Salespersons.Where(m => m.SalespersonID == id).FirstOrDefault();
                    var getSaUser = dbs.Users.Where(m => m.UserID == getSales.UserID).FirstOrDefault();
                    if (getSaUser != null)
                    {
                        getSaUser.IsLocked = true;
                    }
                    dbs.SaveChanges();
                    return RedirectToAction("ViewSalesperson", "Admin");

                case "Supplier":
                    CASDatabaseEntities dbss = new CASDatabaseEntities();
                    var getSupplier = dbss.Suppliers.Where(m => m.SupplierID == id).FirstOrDefault();
                    var getSUser = dbss.Users.Where(m => m.UserID == getSupplier.UserID).FirstOrDefault();
                    if (getSUser != null)
                    {
                        getSUser.IsLocked = true;
                    }
                    dbss.SaveChanges();
                    return RedirectToAction("ViewSupplier", "Admin");

                default:
                    return RedirectToAction("Index", "Admin");
            }
        }

        [Authorize]
        public ActionResult ViewDrugs()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<DrugDataModel> list = new List<DrugDataModel>();
            DrugDataModel dt = new DrugDataModel();

            var getDetails = from d in db.Drugs
                             select new
                             {
                                 d.DrugID,
                                 d.DrugName,
                                 d.Manufacturer,
                                 d.MfgDate,
                                 d.ExpDate,
                                 d.QOH,
                                 d.QOHType,
                                 d.Price,
                                 d.DiscountAmount,
                                 d.IsDeleted
                             };
            foreach (var item in getDetails)
            {
                list.Add(new DrugDataModel
                {
                    DrugID = item.DrugID,
                    DrugName = item.DrugName,
                    Manufacturer = item.Manufacturer,
                    MfgDate = item.MfgDate,
                    ExpDate = item.ExpDate,
                    QOH = item.QOH,
                    QOHType = item.QOHType,
                    Price = Convert.ToDouble(item.Price),
                    DiscountAmount = Convert.ToDouble(item.DiscountAmount),
                    IsDeleted = item.IsDeleted,
                });
            }
            dt.ListDrugs = list;

            return View(dt);
        }

        public ActionResult DeleteDrugs(int? ID)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            DrugDataModel dt = new DrugDataModel();

            int id = Convert.ToInt32(ID);
            var getData = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();

            if (getData != null)
            {
                getData.IsDeleted = true;
                dt.IsDeleted = getData.IsDeleted;
                db.SaveChanges();
            }
            return RedirectToAction("ViewDrugs", "Admin");
        }
    }
}