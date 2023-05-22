using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient

        [Authorize]
        public ActionResult Index()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            var id = Convert.ToInt32(Session["UserID"]); ;
            var checkName = db.Patients.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.PatientID;
            return View();
        }

        [Authorize]
        public ActionResult EditDetails()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            PatientDataModel dt = new PatientDataModel();

            var id = Convert.ToInt32(Session["UserID"]); ;
            var getData = db.Patients.Where(m => m.UserID == id).FirstOrDefault();

            if (getData != null)
            {
                dt.FirstName = getData.FirstName;
                dt.LastName = getData.LastName;
                dt.Gender = getData.Gender;
                dt.DOB = Convert.ToDateTime(getData.DOB);
                dt.Address = getData.Address;
                dt.ContactNo = getData.ContactNo;
                dt.EmgContactName = getData.EmgContactName;
                dt.EmgContactNo = getData.EmgContactNo;
            }
            else
            {
                dt.FirstName = null;
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditDetails(PatientDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Patient objPatient = new Patient();

            var id = Convert.ToInt32(Session["UserID"]); ;
            var getData = db.Patients.Where(m => m.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    getData.FirstName = dt.FirstName;
                    getData.LastName = dt.LastName;
                    getData.Gender = dt.Gender;
                    getData.DOB = dt.DOB;
                    getData.Address = dt.Address;
                    getData.ContactNo = dt.ContactNo;
                    getData.EmgContactName = dt.EmgContactName;
                    getData.EmgContactNo = dt.EmgContactNo;
                }
                else
                {
                    objPatient.FirstName = dt.FirstName;
                    objPatient.LastName = dt.LastName;
                    objPatient.Gender = dt.Gender;
                    objPatient.DOB = dt.DOB;
                    objPatient.Address = dt.Address;
                    objPatient.ContactNo = dt.ContactNo;
                    objPatient.EmgContactName = dt.EmgContactName;
                    objPatient.EmgContactNo = dt.EmgContactNo;
                    db.Patients.Add(objPatient);
                }
                db.SaveChanges();
            }
            var checkName = db.Patients.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.PatientID;
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
            var getData = db.Patients.Where(a => a.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    var getInfo = (from u in db.Users
                                   where (u.UserID == id)
                                   select new
                                   {
                                       u.EmailID,
                                       u.Password,
                                   }).FirstOrDefault();
                    if (dt.OldPassword == dt.NewPassword)
                    {
                        ViewBag.text = "New Password Cannot Be Same As Old Password";
                    }
                    else
                    {
                        if (getInfo.Password == dt.OldPassword)
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
        public ActionResult MakeAppointments()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            AppointmentDataModel dt = new AppointmentDataModel();
            /*List<SelectListItem> list = new List<SelectListItem>();

            var getData = db.SpecializationDatas.ToList();

            foreach (var item in getData)
            {
                list.Add(new SelectListItem
                {
                    Text = item.SpecializationName,
                    Value = item.SpecializationID.ToString()
                });
            }
            dt.ListSpecialization = list;*/
            GetLists(dt);
            dt.AppointmentDate = DateTime.Now;
            return View(dt);
        }

        [HttpPost]
        public ActionResult MakeAppointments(AppointmentDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Appointment objAppointment = new Appointment();
         
            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Patients.Where(m => m.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                GetLists(dt);
                if (getData != null)
                {
                    if (dt.AppointmentDate.DayOfWeek == 0 || (dt.AppointmentDate.Hour < 9 || dt.AppointmentDate.Hour > 17))
                    {
                        ViewBag.text = "Please Enter Time Between 9 AM and 5 PM";
                    }
                    else if (dt.AppointmentDate.Date < DateTime.Today)
                    {
                        ViewBag.text = "Please Enter a Valid Date";
                    }
                    else
                    {
                        objAppointment.PatientID = getData.PatientID;
                        objAppointment.PhysicianID = dt.PhysicianID;
                        objAppointment.Subject = dt.Subject;
                        objAppointment.Description = dt.Description;
                        objAppointment.AppointmentDate = dt.AppointmentDate;
                        objAppointment.AppointmentStatus = "Requested";

                        db.Appointments.Add(objAppointment);
                        db.SaveChanges();
                        ViewBag.text = "Appointment Requested";
                    }
                }
                else
                {
                    ViewBag.text = "Cannot Request Appointment";
                }
            }
            return View(dt);
        }

        public JsonResult GetPhysicians(int id)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SelectListItem> list = new List<SelectListItem>();
            var getData = from p in db.Physicians
                          where p.SpecializationID == id && p.CurrentStatus == "Active"
                          select new
                          {
                              p.PhysicianID,
                              p.FirstName,
                              p.LastName
                          };
            foreach (var item in getData)
            {
                list.Add(new SelectListItem
                {
                    Text = item.FirstName + " " + item.LastName,
                    Value = item.PhysicianID.ToString()
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private void GetLists(AppointmentDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SelectListItem> lists = new List<SelectListItem>();
            List<SelectListItem> listp = new List<SelectListItem>();
            var getSpecialization = db.SpecializationDatas.ToList();
            foreach (var item in getSpecialization)
            {
                lists.Add(new SelectListItem
                {
                    Text = item.SpecializationName,
                    Value = item.SpecializationID.ToString()
                });
            }
            dt.ListSpecialization = lists;
            if(dt.ListSpecialization != null)
            {
                var getPhysician = db.Physicians.ToList();

                foreach (var item in getPhysician)
                {
                    listp.Add(new SelectListItem
                    {
                        Text = item.FirstName + " " + item.LastName,
                        Value = item.PhysicianID.ToString()
                    });
                }
                dt.ListPhysician = listp;
            }           
        }

        [Authorize]
        public ActionResult ViewAppointments()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<AppointmentDataModel> list = new List<AppointmentDataModel>();
            AppointmentDataModel dt = new AppointmentDataModel();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Patients.Where(m => m.UserID == id).FirstOrDefault();

            if(getData != null)
            {
                var getDetails = from a in db.Appointments
                                 join p in db.Physicians
                                 on a.PhysicianID equals p.PhysicianID
                                 where a.PatientID == getData.PatientID
                                 select new
                                 {
                                     p.FirstName,
                                     p.LastName,
                                     a.Subject,
                                     a.Description,
                                     a.AppointmentDate,
                                     a.AppointmentStatus
                                 };

                foreach (var item in getDetails)
                {
                    list.Add(new AppointmentDataModel
                    {
                        PhysicianName = item.FirstName + " " + item.LastName,
                        Subject = item.Subject,
                        Description = item.Description,
                        AppointmentDate = item.AppointmentDate,
                        AppointmentStatus = item.AppointmentStatus,
                    });
                }
                dt.ListData = list;
            }
            return View(dt);
        }

        [Authorize]
        public ActionResult SendMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<AppointmentDataModel> list = new List<AppointmentDataModel>();
            AppointmentDataModel dt = new AppointmentDataModel();

            int ID = Convert.ToInt32(Session["ID"]); ;

            var getDetails = from a in db.Appointments
                             join p in db.Physicians
                             on a.PhysicianID equals p.PhysicianID
                             where (a.PatientID == ID && a.AppointmentStatus == "Accepted")
                             select new
                             {
                                 p.FirstName,
                                 p.LastName,
                                 a.AppointmentID,
                                 a.AppointmentDate
                             };

            foreach (var item in getDetails)
            {
                list.Add(new AppointmentDataModel
                {
                    AppointmentID = item.AppointmentID,
                    PhysicianName = item.FirstName + " " + item.LastName,
                    AppointmentDate = item.AppointmentDate,
                });
            }
            dt.ListData = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult Inbox()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inbox(int? ID, InboxDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Inbox objInbox = new Inbox();

            var id = Convert.ToInt32(ID);
            var getData = db.Appointments.Where(m => m.AppointmentID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    var physicianData = (from a in db.Appointments
                                         join p in db.Physicians
                                         on a.PhysicianID equals p.PhysicianID
                                         join u in db.Users
                                         on p.UserID equals u.UserID
                                         where a.AppointmentID == id
                                         select new
                                         {
                                             u.EmailID
                                         }).FirstOrDefault();

                    string ToEmailID = physicianData.EmailID;

                    objInbox.FromEmailID = Session["EmailID"].ToString();
                    objInbox.ToEmailID = ToEmailID;
                    objInbox.Subject = dt.Subject;
                    objInbox.MessageDetail = dt.MessageDetail;
                    objInbox.MessageDate = DateTime.Now;
                    objInbox.IsRead = false;

                    db.Inboxes.Add(objInbox);
                    db.SaveChanges();
                    ViewBag.text = "Message Sent";
                }
            }
            else
            {
                ViewBag.text = "Message Not Sent";
            }
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<InboxDataModel> list = new List<InboxDataModel>();
            InboxDataModel dt = new InboxDataModel();

            string thisEmail = Session["EmailID"].ToString();
            var getDetails = (from u in db.Users
                              join p in db.Physicians
                              on u.UserID equals p.UserID
                              join i in db.Inboxes
                              on u.EmailID equals i.ToEmailID
                              where (i.ReplyID == 0 && i.FromEmailID == thisEmail)
                              select new
                              {
                                  p.FirstName,
                                  p.LastName,
                                  i.MessageID,
                                  i.Subject,
                                  i.MessageDetail,
                                  i.MessageDate
                              });
            foreach (var item in getDetails)
            {
                list.Add(new InboxDataModel
                {
                    PhysicianName = item.FirstName + " " + item.LastName,
                    MessageID = item.MessageID,
                    Subject = item.Subject,
                    MessageDetail = item.MessageDetail,
                    MessageDate = item.MessageDate
                });
            }
            dt.ListData = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewAllMessages(int? ID)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<InboxDataModel> list = new List<InboxDataModel>();
            InboxDataModel dt = new InboxDataModel();

            int id = Convert.ToInt32(ID);
            var getData = from i in db.Inboxes
                          where i.ReplyID == id
                          select new
                          {
                              i.FromEmailID,
                              i.MessageDetail,
                              i.MessageDate
                          };
            if (getData != null)
            {
                foreach (var item in getData)
                {
                    list.Add(new InboxDataModel
                    {
                        FromEmailID = item.FromEmailID,
                        MessageDetail = item.MessageDetail,
                        MessageDate = item.MessageDate
                    });
                }
                dt.ListData = list;
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult ViewAllMessages(int? ID, InboxDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Inbox objInbox = new Inbox();

            var id = Convert.ToInt32(ID);
            var getData = db.Inboxes.Where(m => m.MessageID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    objInbox.FromEmailID = getData.FromEmailID;
                    objInbox.ToEmailID = getData.ToEmailID;
                    objInbox.MessageDetail = dt.MessageDetail;
                    objInbox.MessageDate = DateTime.Now;
                    objInbox.ReplyID = id;
                    objInbox.IsRead = false;

                    db.Inboxes.Add(objInbox);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.text = "Failed";
                }
            }
            return RedirectToAction("ViewAllMessages", "Patient");
        }

        [Authorize]
        public ActionResult OrderDrugs()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            PatientOrderModel dt = new PatientOrderModel();

            List<SelectListItem> list = new List<SelectListItem>();
            var getDrugDetails = from d in db.Drugs
                                 where d.IsDeleted == false
                                 select new
                                 {
                                     d.DrugID,
                                     d.DrugName,
                                 };
            foreach (var item in getDrugDetails)
            {
                list.Add(new SelectListItem
                {
                    Text = item.DrugName,
                    Value = item.DrugID.ToString()
                });
            }
            dt.ListDrug = list;
            return View(dt);
        }

        [HttpPost]
        public ActionResult OrderDrugs(PatientOrderModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            PatientOrderDetail objOrder = new PatientOrderDetail();

            Random random = new Random();

            List<SelectListItem> list = new List<SelectListItem>();
            var getDrugDetails = from d in db.Drugs
                                 where d.IsDeleted == false
                                 select new
                                 {
                                     d.DrugID,
                                     d.DrugName,
                                 };
            foreach (var item in getDrugDetails)
            {
                list.Add(new SelectListItem
                {
                    Text = item.DrugName,
                    Value = item.DrugID.ToString()
                });
            }
            dt.ListDrug = list;

            var getQuantity = db.Drugs.Where(m => m.DrugID == dt.DrugID).FirstOrDefault();
            dt.QuantityAvailable = getQuantity.QOH;
            dt.QuantityType = getQuantity.QOHType;
            dt.DrugID = getQuantity.DrugID;

            var getSales = db.Salespersons.SingleOrDefault();
            dt.SalespersonID = getSales.SalespersonID;

            if (ModelState.IsValid)
            {
                if (dt.Quantity == 0)
                {
                    ViewBag.text = "Quantity Must Not be Zero.";
                }
                else if (dt.Quantity <= dt.QuantityAvailable)
                {
                    objOrder.DrugID = dt.DrugID;
                    objOrder.Quantity = dt.Quantity;
                    objOrder.OrderNumber = random.Next();
                    objOrder.OrderDate = DateTime.Today;
                    objOrder.OrderStatus = "Requested";
                    objOrder.PatientID = Convert.ToInt32(Session["ID"]);
                    objOrder.SalespersonID = dt.SalespersonID;

                    db.PatientOrderDetails.Add(objOrder);

                    getQuantity.QOH -= dt.Quantity;
                    if (getQuantity.QOH == 0)
                    {
                        getQuantity.IsDeleted = true;
                    }
                    db.SaveChanges();
                    ViewBag.text = "Order Successfully Placed";
                }
                else if (dt.Quantity > dt.QuantityAvailable)
                {
                    ViewBag.text = "Quantity Exceeded";
                }
            }
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewOrders()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<PatientOrderModel> list = new List<PatientOrderModel>();
            PatientOrderModel dt = new PatientOrderModel();

            int id = Convert.ToInt32(Session["ID"]);

            var getDetails = from o in db.PatientOrderDetails
                             join d in db.Drugs
                             on o.DrugID equals d.DrugID
                             where o.PatientID == id
                             select new
                             {
                                 d.DrugName,
                                 o.OrderNumber,
                                 o.OrderDate,
                                 o.OrderStatus,
                                 o.Quantity
                             };

            foreach (var item in getDetails)
            {
                list.Add(new PatientOrderModel
                {
                    DrugName = item.DrugName,
                    OrderNumber = item.OrderNumber,
                    OrderDate = Convert.ToDateTime(item.OrderDate),
                    OrderStatus = item.OrderStatus,
                    Quantity = Convert.ToInt32(item.Quantity)
                });
            }
            dt.ListOrder = list;
            return View(dt);
        }

    }
}