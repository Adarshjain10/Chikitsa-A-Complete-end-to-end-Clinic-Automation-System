using ClinicalAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        [Authorize]
        public ActionResult Index()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            var id = Convert.ToInt32(Session["UserID"]);
            var checkName = db.Suppliers.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.SupplierID;
            return View();
        }

        [Authorize]
        public ActionResult EditDetails()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            SupplierDataModel dt = new SupplierDataModel();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Suppliers.Where(a => a.UserID == id).FirstOrDefault();

            if (getData != null)
            {
                dt.FirstName = getData.FirstName;
                dt.LastName = getData.LastName;
                dt.CompanyName = getData.CompanyName;
                dt.CompanyAddress = getData.CompanyAddress;
                dt.CurrentStatus = getData.CurrentStatus;
            }
            else
            {
                dt.FirstName = null;
            }
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditDetails(SupplierDataModel dt)
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            Supplier objSupplier = new Supplier();

            var id = Convert.ToInt32(Session["UserID"]);
            var getData = db.Suppliers.Where(a => a.UserID == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (getData != null)
                {
                    getData.FirstName = dt.FirstName;
                    getData.LastName = dt.LastName;
                    getData.CompanyName = dt.CompanyName;
                    getData.CompanyAddress = dt.CompanyAddress;
                    getData.CurrentStatus = dt.CurrentStatus;
                }
                else
                {
                    objSupplier.FirstName = dt.FirstName;
                    objSupplier.LastName = dt.LastName;
                    objSupplier.CompanyName = dt.CompanyName;
                    objSupplier.CompanyAddress = dt.CompanyAddress;
                    objSupplier.CurrentStatus = dt.CurrentStatus;

                    db.Suppliers.Add(objSupplier);
                }
                db.SaveChanges();
            }
            var checkName = db.Suppliers.Where(a => a.UserID == id).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["Name"] = checkName.FirstName;
            }
            else
            {
                Session["Name"] = null;
            }
            Session["ID"] = checkName.SupplierID;
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
            var getData = db.Suppliers.Where(a => a.UserID == id).FirstOrDefault();
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
        public ActionResult ViewOrders()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<SalesOrderModel> list = new List<SalesOrderModel>();
            SalesOrderModel dt = new SalesOrderModel();

            int id = Convert.ToInt32(Session["ID"]);

            var getData = from so in db.SalespersonOrderDetails
                          where so.SupplierID == id
                          select new
                          {
                              so.DrugName,
                              so.SalespersonOrderID,
                              so.OrderNumber,
                              so.OrderDate,
                              so.Quantity,
                              so.OrderStatus
                          };
            foreach (var item in getData)
            {
                list.Add(new SalesOrderModel
                {
                    SalespersonOrderID = item.SalespersonOrderID,
                    OrderNumber = item.OrderNumber,
                    DrugName = item.DrugName,
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
            SalesOrderModel dt = new SalesOrderModel();
            SalespersonOrderDetail objOrder = new SalespersonOrderDetail();
            DrugDelivery objDelivery = new DrugDelivery();

            var id = Convert.ToInt32(ID);
            var getData = db.SalespersonOrderDetails.Where(m => m.SalespersonOrderID == id).FirstOrDefault();

            if (str == "Deliver")
            {
                getData.OrderStatus = "Delivered";

                objDelivery.SalespersonOrderID = getData.SalespersonOrderID;
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
            return RedirectToAction("ViewOrders", "Supplier");
        }

        [Authorize]
        public ActionResult ViewMessages()
        {
            CASDatabaseEntities db = new CASDatabaseEntities();
            List<InboxDataModel> list = new List<InboxDataModel>();
            InboxDataModel dt = new InboxDataModel();

            string thisEmail = Session["EmailID"].ToString();
            var getDetails = (from u in db.Users
                              join s in db.Salespersons
                              on u.UserID equals s.UserID
                              join i in db.Inboxes
                              on u.EmailID equals i.FromEmailID
                              where (i.ReplyID == 0 && i.ToEmailID == thisEmail)
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

            return RedirectToAction("ViewAllMessages", "Supplier");
        }
    }
}