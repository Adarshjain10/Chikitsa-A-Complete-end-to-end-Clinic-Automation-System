using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class SalespersonController : Controller
    {
        // GET: Salesperson

        [Authorize]
        public ActionResult Index()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            var id = Convert.ToInt32(Session["UserID"]);
            var checkName = db.Salespersons.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.SalespersonID;
            return View();
        }

        [Authorize]
        public ActionResult EditDetails()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            SalesDataModel dt = new SalesDataModel();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Salespersons.Where(a => a.UserID == id).FirstOrDefault();

            if (getData != null)
            {
                dt.FirstName = getData.FirstName;
                dt.LastName = getData.LastName;
                dt.CurrentStatus = getData.CurrentStatus;
            }
            else
            {
                dt.FirstName = null;
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditDetails(SalesDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Salesperson objSalesperson = new Salesperson();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Salespersons.Where(a => a.UserID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    getData.FirstName = dt.FirstName;
                    getData.LastName = dt.LastName;
                    getData.CurrentStatus = dt.CurrentStatus;
                }
                else
                {
                    objSalesperson.FirstName = dt.FirstName;
                    objSalesperson.LastName = dt.LastName;
                    objSalesperson.CurrentStatus = dt.CurrentStatus;

                    db.Salespersons.Add(objSalesperson);
                }
                db.SaveChanges();
            }
            var checkName = db.Salespersons.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.SalespersonID;
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
            var getData = db.Salespersons.Where(a => a.UserID == id).FirstOrDefault();
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
        public ActionResult AddDrugs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDrugs(DrugDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Drug objDrug = new Drug();

            if(ModelState.IsValid)
            {
                var getData = db.Drugs.Where(m => m.DrugName == dt.DrugName).FirstOrDefault();
                if (getData != null)
                {
                    ViewBag.Text = "Drug Already Present. Only Updation Allowed";
                }
                else
                {
                    if(dt.QOH == 0)
                    {
                        ViewBag.Text = "New Drug Cannot Be Entered With ZERO Quantity.";
                    }
                    else
                    {
                        if(dt.MfgDate > DateTime.Today || dt.ExpDate < DateTime.Today)
                        {
                            ViewBag.Text = "Invalid Manufacturing or Expiry Date";
                        }
                        else
                        {
                            objDrug.DrugName = dt.DrugName;
                            objDrug.Manufacturer = dt.Manufacturer;
                            objDrug.Substitutions = dt.Substitutions;
                            objDrug.MfgDate = dt.MfgDate;
                            objDrug.ExpDate = dt.ExpDate;
                            objDrug.Uses = dt.Uses;
                            objDrug.SideEffects = dt.SideEffects;
                            objDrug.NotRecommended = dt.NotRecommended;
                            objDrug.QOH = dt.QOH;
                            objDrug.QOHType = dt.QOHType;
                            objDrug.Price = Convert.ToDecimal(dt.Price);
                            objDrug.DiscountAmount = Convert.ToDecimal(dt.DiscountAmount);
                            objDrug.IsDeleted = false;

                            db.Drugs.Add(objDrug);
                            db.SaveChanges();

                            ViewBag.Text = "Drug Inserted Successfully";
                        }                        
                    }                    
                }
            }
            return View(dt);
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

        [Authorize]
        public ActionResult EditDrugs(int? ID)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            DrugDataModel dt = new DrugDataModel();

            int id = Convert.ToInt32(ID);
            var getData = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();

            if (getData != null)
            {
                dt.DrugName = getData.DrugName;
                dt.Manufacturer = getData.Manufacturer;
                dt.Substitutions = getData.Substitutions;
                dt.Uses = getData.Uses;
                dt.SideEffects = getData.SideEffects;
                dt.NotRecommended = getData.NotRecommended;
                dt.MfgDate = getData.MfgDate;
                dt.ExpDate = getData.ExpDate;
                dt.QOH = getData.QOH;
                dt.QOHType = getData.QOHType;
                dt.Price = Convert.ToDouble(getData.Price);
                dt.DiscountAmount = Convert.ToDouble(getData.DiscountAmount);
                dt.IsDeleted = getData.IsDeleted;
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditDrugs(int? ID, DrugDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();

            int id = Convert.ToInt32(ID);
            var getData = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    if(getData.QOH == 0 && dt.QOH > 0)
                    {
                        getData.IsDeleted = false;
                    }
                    else if(dt.QOH > 0 && dt.IsDeleted == false)
                    {
                        getData.IsDeleted = false;
                    }
                    else
                    {
                        getData.IsDeleted = true;
                    }
                    getData.DrugName = dt.DrugName;
                    getData.Manufacturer = dt.Manufacturer;
                    getData.Substitutions = dt.Substitutions;
                    getData.Uses = dt.Uses;
                    getData.SideEffects = dt.SideEffects;
                    getData.NotRecommended = dt.NotRecommended;
                    getData.MfgDate = dt.MfgDate;
                    getData.ExpDate = dt.ExpDate;
                    getData.QOH = dt.QOH;
                    getData.QOHType = dt.QOHType;
                    getData.Price = Convert.ToDecimal(dt.Price);
                    getData.DiscountAmount = Convert.ToDecimal(dt.DiscountAmount);
                    
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ViewDrugs", "Salesperson");
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
            return RedirectToAction("ViewDrugs", "Salesperson");
        }

        [Authorize]
        public ActionResult ViewPatientOrders()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<PatientOrderModel> list = new List<PatientOrderModel>();
            PatientOrderModel dt = new PatientOrderModel();

            var getData = from po in db.PatientOrderDetails
                          join d in db.Drugs
                          on po.DrugID equals d.DrugID
                          join p in db.Patients
                          on po.PatientID equals p.PatientID
                          select new
                          {
                              p.FirstName,
                              p.LastName,
                              d.DrugName,
                              po.PatientOrderID,
                              po.OrderNumber,
                              po.OrderDate,
                              po.Quantity,
                              po.OrderStatus
                          };
            foreach(var item in getData)
            {
                list.Add(new PatientOrderModel
                {
                    PatientOrderID = item.PatientOrderID,
                    OrderNumber = item.OrderNumber,
                    DrugName = item.DrugName,
                    PatientName = item.FirstName + " " + item.LastName,
                    Quantity = Convert.ToInt32(item.Quantity),
                    OrderStatus = item.OrderStatus,
                    OrderDate = Convert.ToDateTime(item.OrderDate)
                });
            }
            dt.ListOrder = list;
            return View(dt);
        }

        public ActionResult UpdateOrder(int? ID, string str)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            PatientOrderModel dt = new PatientOrderModel();
            PatientOrderDetail objOrder = new PatientOrderDetail();
            DrugDelivery objDelivery = new DrugDelivery();

            var id = Convert.ToInt32(ID);
            var getData = db.PatientOrderDetails.Where(m => m.PatientOrderID == id).FirstOrDefault();

            if(str == "Deliver")
            {
                getData.OrderStatus = "Delivered";

                objDelivery.PatientOrderID = getData.PatientOrderID;
                objDelivery.DeliveryDate = DateTime.Today;
                db.DrugDeliveries.Add(objDelivery);

                Session["Deliver"] = "Delivered";
            }
            else
            {
                getData.OrderStatus = "Rejected";
            }
            dt.OrderStatus = getData.OrderStatus;
            db.SaveChanges();
            return RedirectToAction("ViewPatientOrders", "Salesperson");
        }

        [Authorize]
        public ActionResult PlaceOrders()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SupplierDataModel> list = new List<SupplierDataModel>();
            SupplierDataModel dt = new SupplierDataModel();

            var getData = from s in db.Suppliers
                          where s.CurrentStatus == "Active"
                          select new
                          {
                              s.SupplierID,
                              s.FirstName,
                              s.LastName,
                              s.CompanyName,
                              s.CompanyAddress
                          };
            foreach(var item in getData)
            {
                list.Add(new SupplierDataModel
                {
                    SupplierID = item.SupplierID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    CompanyName = item.CompanyName,
                    CompanyAddress = item.CompanyAddress
                });
            }
            dt.ListSupplier = list;
            return View(dt);
        }

        [Authorize]
        public ActionResult GetOrders(int? ID)
        {
            SalesOrderModel dt = new SalesOrderModel();
            dt.SupplierID = Convert.ToInt32(ID);
            return View(dt);
        }

        [HttpPost]
        public ActionResult GetOrders(int? ID, SalesOrderModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            SalespersonOrderDetail objOrder = new SalespersonOrderDetail();
            Random random = new Random();
            int id = Convert.ToInt32(Session["ID"]);

            if (ModelState.IsValid)
            {
                objOrder.SupplierID = Convert.ToInt32(ID);
                objOrder.SalespersonID = id;
                objOrder.DrugName = dt.DrugName;
                objOrder.Quantity = dt.Quantity;
                objOrder.OrderNumber = random.Next();
                objOrder.OrderDate = DateTime.Today;
                objOrder.OrderStatus = "Requested";

                db.SalespersonOrderDetails.Add(objOrder);
                db.SaveChanges();
            }
            return View();
        }

        [Authorize]
        public ActionResult ViewOrders()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SalesOrderModel> list = new List<SalesOrderModel>();
            SalesOrderModel dt = new SalesOrderModel();

            int id = Convert.ToInt32(Session["ID"]);

            var getDetails = from o in db.SalespersonOrderDetails
                             join s in db.Suppliers
                             on o.SupplierID equals s.SupplierID
                             select new
                             {
                                 s.FirstName,
                                 s.LastName,
                                 s.CompanyName,
                                 o.DrugName,
                                 o.OrderNumber,
                                 o.OrderDate,
                                 o.OrderStatus,
                                 o.Quantity
                             };

            foreach (var item in getDetails)
            {
                list.Add(new SalesOrderModel
                {
                    SupplierName = item.FirstName + " " + item.LastName,
                    SupplierCompanyName = item.CompanyName,
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

            return RedirectToAction("ViewAllMessages", "Salesperson");
        }

        [Authorize]
        public ActionResult SendMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SalesOrderModel> list = new List<SalesOrderModel>();
            SalesOrderModel dt = new SalesOrderModel();

            int ID = Convert.ToInt32(Session["ID"]); ;

            var getDetails = from o in db.SalespersonOrderDetails
                             join s in db.Suppliers
                             on o.SupplierID equals s.SupplierID
                             where (o.SalespersonID == ID && (o.OrderStatus == "Requested" || o.OrderStatus == "Delivered"))
                             select new
                             {
                                 s.FirstName,
                                 s.LastName,
                                 o.SalespersonOrderID,
                                 o.OrderNumber,
                                 o.OrderDate,
                                 o.DrugName,
                                 o.Quantity,
                                 o.OrderStatus
                             };

            foreach (var item in getDetails)
            {
                list.Add(new SalesOrderModel
                {
                    SalespersonOrderID = item.SalespersonOrderID,
                    SupplierName = item.FirstName + " " + item.LastName,
                    OrderNumber = item.OrderNumber,
                    OrderDate = Convert.ToDateTime(item.OrderDate),
                    DrugName = item.DrugName,
                    Quantity = Convert.ToInt32(item.Quantity),
                    OrderStatus = item.OrderStatus
                });
            }
            dt.ListOrder = list;
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
            var getData = db.SalespersonOrderDetails.Where(m => m.SalespersonOrderID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    var supplierData = (from o in db.SalespersonOrderDetails
                                         join s in db.Suppliers
                                         on o.SupplierID equals s.SupplierID
                                         join u in db.Users
                                         on s.UserID equals u.UserID
                                         where o.SalespersonOrderID == id
                                         select new
                                         {
                                             u.EmailID
                                         }).FirstOrDefault();

                    string ToEmailID = supplierData.EmailID;

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
        public ActionResult ViewSupplierMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<InboxDataModel> list = new List<InboxDataModel>();
            InboxDataModel dt = new InboxDataModel();

            string thisEmail = Session["EmailID"].ToString();
            var getDetails = (from u in db.Users
                              join s in db.Suppliers
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
                    SupplierName = item.FirstName + " " + item.LastName,
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
        public ActionResult ViewAllSupplierMessages(int? ID)
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
        public ActionResult ViewAllSupplierMessages(int? ID, InboxDataModel dt)
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
            return RedirectToAction("ViewAllSupplierMessages", "Salesperson");
        }

    }
}