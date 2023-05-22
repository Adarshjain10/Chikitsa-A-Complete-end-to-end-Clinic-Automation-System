using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class PhysicianController : Controller
    {
        // GET: Physician

        [Authorize]
        public ActionResult Index()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            var id = Convert.ToInt32(Session["UserID"]);
            var checkName = db.Physicians.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.PhysicianID;
            return View();
        }

        [Authorize]
        public ActionResult EditDetails()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            PhysicianDataModel dt = new PhysicianDataModel();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Physicians.Where(a => a.UserID == id).FirstOrDefault();

            if (getData != null)
            {
                dt.FirstName = getData.FirstName;
                dt.LastName = getData.LastName;
                dt.Gender = getData.Gender;
                dt.SpecializationID = getData.SpecializationID;
                dt.TotalExperience = getData.TotalExperience;
                dt.Education = getData.Education;
                dt.CurrentStatus = getData.CurrentStatus;
            }
            else
            {
                dt.FirstName = null;
            }

            List<SelectListItem> list = new List<SelectListItem>();

            var getSpecData = db.SpecializationDatas.ToList();

            foreach (var item in getSpecData)
            {
                list.Add(new SelectListItem
                {
                    Text = item.SpecializationName,
                    Value = item.SpecializationID.ToString()
                });
            }
            dt.ListSpecialization = list;
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditDetails(PhysicianDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Physician objPhysician = new Physician();
            List<SelectListItem> list = new List<SelectListItem>();

            var getSpecData = db.SpecializationDatas.ToList();

            foreach (var item in getSpecData)
            {
                list.Add(new SelectListItem
                {
                    Text = item.SpecializationName,
                    Value = item.SpecializationID.ToString()
                });
            }
            dt.ListSpecialization = list;

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Physicians.Where(a => a.UserID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    getData.FirstName = dt.FirstName;
                    getData.LastName = dt.LastName;
                    getData.Gender = dt.Gender;
                    getData.SpecializationID = dt.SpecializationID;
                    getData.TotalExperience = dt.TotalExperience;
                    getData.Education = dt.Education;
                    getData.CurrentStatus = dt.CurrentStatus;
                }
                else
                {
                    objPhysician.FirstName = dt.FirstName;
                    objPhysician.LastName = dt.LastName;
                    objPhysician.Gender = dt.Gender;
                    objPhysician.TotalExperience = dt.TotalExperience;
                    objPhysician.SpecializationID = dt.SpecializationID;
                    objPhysician.Education = dt.Education;
                    objPhysician.CurrentStatus = dt.CurrentStatus;

                    db.Physicians.Add(objPhysician);
                }
                db.SaveChanges();
            }
            var checkName = db.Physicians.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.PhysicianID;
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
            var getData = db.Physicians.Where(a => a.UserID == id).FirstOrDefault();
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
        public ActionResult ViewAppointment()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<AppointmentDataModel> list = new List<AppointmentDataModel>();
            AppointmentDataModel dt = new AppointmentDataModel();

            int ID = Convert.ToInt32(Session["ID"]); 

            var getDetails = (from a in db.Appointments
                             join p in db.Patients
                             on a.PatientID equals p.PatientID
                             where a.PhysicianID == ID
                             select new
                             {
                                 p.FirstName,
                                 p.LastName,
                                 p.Gender,
                                 a.AppointmentID,
                                 a.Subject,
                                 a.Description,
                                 a.AppointmentDate,
                                 a.AppointmentStatus
                             }).OrderBy(a => a.AppointmentDate);

            foreach (var item in getDetails)
            {
                list.Add(new AppointmentDataModel
                {
                    AppointmentID = item.AppointmentID,
                    PatientName = item.FirstName + " " + item.LastName,
                    PatientGender = item.Gender,
                    Subject = item.Subject,
                    Description = item.Description,
                    AppointmentDate = item.AppointmentDate,
                    AppointmentStatus = item.AppointmentStatus,
                });
            }
            dt.ListData = list;

            return View(dt);
        }

        public ActionResult UpdateAppointment(int? ID, string str)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            AppointmentDataModel dt = new AppointmentDataModel();
            Appointment objAppointment = new Appointment();

            var id = Convert.ToInt32(ID);
            var getData = db.Appointments.Where(m => m.AppointmentID == id).FirstOrDefault();

            if (str == "Accept")
            {
                getData.AppointmentStatus = "Accepted";
                Session["Accept"] = "Accepted";
            }
            else
            {
                getData.AppointmentStatus = "Rejected";
            }
            dt.AppointmentStatus = getData.AppointmentStatus;
            db.SaveChanges();
            return RedirectToAction("ViewAppointment", "Physician");
        }

        [Authorize]
        public ActionResult History()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<PatientDataModel> list = new List<PatientDataModel>();
            PatientDataModel dt = new PatientDataModel();

            int ID = Convert.ToInt32(Session["ID"]);
            //DateTime today = DateTime.Today.Parse("yyyy/MM/dd");

            var getDetails = from a in db.Appointments
                             join p in db.Patients
                             on a.PatientID equals p.PatientID
                             where (a.PhysicianID == ID && a.AppointmentStatus == "Accepted" &&  DateTime.Compare(a.AppointmentDate, DateTime.Now) < 0 || DateTime.Compare(a.AppointmentDate, DateTime.Now) == 0)
                             select new
                             {
                                 p.PatientID,
                                 p.FirstName,
                                 p.LastName,
                                 p.Gender,
                                 p.DOB,
                                 p.History
                             };

            foreach (var item in getDetails)
            {
                list.Add(new PatientDataModel
                {
                    PatientID = item.PatientID,
                    FirstName = item.FirstName, 
                    LastName = item.LastName,
                    Gender = item.Gender,
                    Age = Convert.ToInt32(((DateTime.Today).Subtract(Convert.ToDateTime(item.DOB)).TotalDays)/365.25),
                    History = item.History
                });
            }
            dt.ListPatient = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult UpdateHistory(int? ID)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            PatientDataModel dt = new PatientDataModel();

            int id = Convert.ToInt32(ID);
            var getData = db.Patients.Where(m => m.PatientID == id).Select(a => new { a.History }).FirstOrDefault();

            int pid = Convert.ToInt32(Session["ID"]);
            var getName = db.Physicians.Where(m => m.PhysicianID == pid).Select(a => new { a.FirstName, a.LastName }).FirstOrDefault();

            if (getData != null)
            {
                dt.History = getData.History + "\n" + DateTime.Today.ToString() + " " + getName.FirstName + " " + getName.LastName + ":";
            }
            else
            {
                dt.History = null + DateTime.Today.ToString() + " " + getName.FirstName + " " + getName.LastName + ":";
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult UpdateHistory(int? ID, PatientDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Patient objPatient = new Patient();

            int id = Convert.ToInt32(ID);
            var getData = db.Patients.Where(m => m.PatientID == id).FirstOrDefault();

            getData.History = dt.History;
            db.SaveChanges();
            return RedirectToAction("History", "Physician");
        }

        [Authorize]
        public ActionResult SendMessages()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessages(InboxDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Inbox objInbox = new Inbox();

            if(ModelState.IsValid)
            {
                var salesData = (from u in db.Users
                                 where u.RoleID == 4
                                 select new
                                 {
                                     u.EmailID
                                 }).SingleOrDefault();
                string ToEmailID = salesData.EmailID;

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
            else
            {
                ViewBag.text = "Message Not Sent";
            }
            return View(dt);
        }

        [Authorize]
        public ActionResult ViewSalesMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<InboxDataModel> list = new List<InboxDataModel>();
            InboxDataModel dt = new InboxDataModel();

            string thisEmail = Session["EmailID"].ToString();
            var getDetails = (from u in db.Users
                              join s in db.Salespersons
                              on u.UserID equals s.UserID
                              join i in db.Inboxes
                              on u.EmailID equals i.ToEmailID
                              where (i.ReplyID == 0 && i.FromEmailID == thisEmail)
                              select new
                              {
                                  s.FirstName,
                                  s.LastName,
                                  i.MessageID,
                                  i.Subject,
                                  i.MessageDetail,
                                  i.MessageDate
                              });
            foreach (var item in getDetails)
            {
                list.Add(new InboxDataModel
                {
                    SalespersonName = item.FirstName + " " + item.LastName,
                    MessageID = item.MessageID,
                    Subject = item.Subject,
                    MessageDetail = item.MessageDetail,
                    MessageDate = item.MessageDate
                });
            }
            dt.ListData = list;
            return View(dt);
        }

        public ActionResult ViewAllSalesMessages(int? ID)
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
        public ActionResult ViewAllSalesMessages(int? ID, InboxDataModel dt)
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
            return RedirectToAction("ViewAllSalesMessages", "Physician");
        }

        [Authorize]
        public ActionResult ViewMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<InboxDataModel> list = new List<InboxDataModel>();
            InboxDataModel dt = new InboxDataModel();

            string thisEmail = Session["EmailID"].ToString();
            var getDetails = (from u in db.Users
                              join p in db.Patients
                              on u.UserID equals p.UserID
                              join i in db.Inboxes
                              on u.EmailID equals i.FromEmailID
                              where (i.ReplyID == 0 && i.ToEmailID == thisEmail)
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
                    PatientName = item.FirstName + " " + item.LastName,
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
                    objInbox.FromEmailID = getData.ToEmailID;
                    objInbox.ToEmailID = getData.FromEmailID;
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

            return RedirectToAction("ViewAllMessages", "Physician");
        }
    }
}