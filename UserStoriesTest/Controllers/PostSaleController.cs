using Helpers;
using Helpers.DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace UserStoriesTest.Controllers
{
    public class PostSaleController : Controller
    {
      
        public PostSale Post = new PostSale();
        Registry registry = new Registry();


        public ActionResult NewPayment()
        {
            bool Valid = Post.VerifyParams(Request.QueryString);
            if (Valid)
                Ledger.Add(Request.QueryString);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult EncodeDirectPayment()
        {
            XDocument document = (XDocument)TempData["document"];
            NameValueCollection param = new NameValueCollection();
            param.Add("PAYID", document.Element("ncresponse").Attribute("PAYID").Value);
            param.Add("CN", document.Element("ncresponse").Attribute("CN").Value);
            param.Add("orderID", document.Element("ncresponse").Attribute("orderID").Value);
            param.Add("currency", document.Element("ncresponse").Attribute("currency").Value);
            param.Add("amount", document.Element("ncresponse").Attribute("amount").Value);
            param.Add("STATUS", document.Element("ncresponse").Attribute("STATUS").Value);

            Ledger.Add(param);
            return RedirectToAction("Index", "Home");
            
        }
        public void StatusChange(string orderID, string STATUS)
        {
            List<Payment> payments = Ledger.GetAllPayments();
            //Find id and status
            string[] values = new string[] { orderID, STATUS };
            //Find index of payment in memory
            int index = payments.FindIndex(p => p.Orderid == values[0]);

            //Update in memory
            Payment tochange = payments.Where(p => p.Orderid == values[0]).FirstOrDefault();
            tochange.Status = values[1];
            //Update in file
            Post.LineChanger(tochange, index);

            StringBuilder sb = new StringBuilder();
            sb.Append(Request.Url.AbsoluteUri);
            sb.Append("testtest :" + orderID + " : " + STATUS);
            string s = sb.ToString();
            System.IO.File.AppendAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/Param.txt"), new string[] { s });
        }
        public ActionResult DirectCapture(string reference)
        {
            Payment payment = Ledger.FindPaymentByPayid(reference);
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param.Add("OPERATION", "SAS");
            param.Add("AMOUNT", (Convert.ToDecimal(payment.Amount) * 100).ToString("0.##"));
            param.Add("PAYID", payment.PayId);
            param.Add("PSPID", "EPhilippeTest");
            param.Add("PSWD", "N4t&1ytqE#");
            param.Add("USERID", "user-API");
            string tohash = Check.StringToHash(param, "Mypassphraseissocool!");
            string hash = Check.GetHash(tohash);
            param.Add("SHASIGN", hash);
            NameValueCollection query = Check.FromDicToCol(param);
            WebClient myWebClient = new WebClient();
            myWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            try
            {
                byte[] response = myWebClient.UploadValues("https://webdev.oglan.local/Ncol/Test/maintenancedirect.asp", query);
                string res = System.Text.Encoding.UTF8.GetString(response);
                XDocument document = XDocument.Parse(res);
                TempData["document"] = document;
                string status = (document.Element("ncresponse").Attribute("STATUS").Value);
                string orderid = (document.Element("ncresponse").Attribute("orderID").Value);
                string[] values = new string[] { orderid, status };
                //Find index of payment in memory
                List<Payment> payments = Ledger.GetAllPayments();
                int index = payments.FindIndex(p => p.Orderid == values[0]);

                //Update in memory
                Payment tochange = payments.Where(p => p.Orderid == values[0]).FirstOrDefault();
                tochange.Status = values[1];
                //Update in file
                Post.LineChanger(tochange, index);
                return RedirectToAction("Index", "Payments");
            }
            catch (WebException e)
            {
                return RedirectToAction("Index", "Payments");
            }
        }
        public void DirectQuery(string payid)
        {
            Payment payment = Ledger.FindPaymentByPayid(payid);
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param.Add("PAYID", payment.PayId);
            param.Add("PSPID", "EPhilippeTest");
            param.Add("PSWD", "N4t&1ytqE#");
            param.Add("USERID", "user-API");
            NameValueCollection query = Check.FromDicToCol(param);
            WebClient myWebClient = new WebClient();
            myWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            try
            {
                byte[] response = myWebClient.UploadValues("https://webdev.oglan.local/Ncol/Test/querydirect.asp", query);
                string res = System.Text.Encoding.UTF8.GetString(response);
                XDocument document = XDocument.Parse(res);
                string newstatus = document.Element("ncresponse").Attribute("STATUS").Value;
                if(newstatus != payment.Status)
                {
                    List<Payment> payments = Ledger.GetAllPayments();
                    int index = payments.FindIndex(p => p.PayId == payid);
                    payment.Status = newstatus;
                    Post.LineChanger(payment, index);
                }
            }
            catch (WebException e)
            {

            }
        }
        public ActionResult GenerateBatch(string[] references)
        {
            List<Payment> payments = new List<Payment>();
            if (references.Count() > 0)
            {
                foreach (string payid in references)
                    {
                        payments.Add(Ledger.FindPaymentByPayid(payid));
                    }
                    List<string> BatchPayments = Post.GenerateBatch(payments);
                    return PartialView("_BatchFile", BatchPayments);
            }
            else
            {
                return PartialView("_BatchFile", new List<string>());
            }
        }
        public ActionResult DisplayBatch()
        {
            List<string> payments = (List<string>)TempData["batch"];
            //return View("BatchFile", payments);
            return Redirect(Url.Action("BatchFile", "PostSale",payments));
        }
        public void StatusChangeTest(string orderID,string STATUS)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Request.Url.AbsoluteUri);
            sb.Append("testtest :" + orderID + " : " + STATUS);
            string s = sb.ToString();
            System.IO.File.AppendAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/Param.txt"), new string[] { s });
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: PostSale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostSale/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostSale/Create
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

        // GET: PostSale/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostSale/Edit/5
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

        // GET: PostSale/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostSale/Delete/5
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
    }
}
