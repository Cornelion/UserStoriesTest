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
        public void DirectCapture(int reference)
        {
            Payment payment = Ledger.FindPaymentByPayid(reference);
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param.Add("OPERATION", "SAS");
            param.Add("AMOUNT", (Convert.ToInt32(payment.Amount) * 100).ToString());
            param.Add("orderID", payment.Orderid);
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
            }
            catch (WebException e)
            {
            }
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
