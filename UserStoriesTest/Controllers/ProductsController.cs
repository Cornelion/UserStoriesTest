using Helpers;
using Helpers.DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;

namespace UserStoriesTest.Controllers
{
    public class ProductsController : Controller
    {

        public List<Product> productsList;
        public Registry registry = new Registry();
        public Order Order
        {
            get { return (Order)Session["Order"]; }
            set { Session["Order"] = value; }
        }
       
        public ProductsController()
        {
            productsList = Catalog.GetProducts();
           
        }
      
        public  PartialViewResult ProductsCount(string methode, int id)
        {
            if (!String.IsNullOrEmpty(methode)){
                switch (methode)
                {
                    case "add":
                        {
                            AddToCart(id);
                            break;
                        }
                    case "remove": default:
                        {
                            RemoveFromCart(id);
                            break;
                        }
                }
            }
            if (Order != null)
            {
                return PartialView("_Order", Order);
            }
           
                return PartialView("_Order", new Order());
            
        }
        public void AddToCart(int id)
        {
            Order.Products.Add(Catalog.CloneProduct(id));
            Session["Order"] = Order;
        }
        public void RemoveFromCart(int id)
        {
            Order.RemoveItem(id);
            Session["Order"] = Order;

        }
        public ActionResult PrepareCheckOut()
        {
            OrderVM final = Check.GenerateOrderVM(Order);
            User user = registry.FindUserByName(System.Web.HttpContext.Current.User.Identity.Name);
            TempData["aliases"]= AliasManager.FindAliasesForUser(user.UserName);
            return RedirectToAction("Index", "Alias");
        }
        public ActionResult CheckOut()
        {   //?? new Alias()
            Alias alias = (Alias)TempData["selectedalias"]  ;
         
            PaymentVM payment = new PaymentVM() {
                order = Check.GenerateOrderVM(Order),
                isdirectlink =true,
                SelectedAlias = alias,
                user = registry.FindUserByName(System.Web.HttpContext.Current.User.Identity.Name)
        } ;
            
            Check.SetParameters(payment);
            TempData["payment"] = payment;
            if (payment.isdirectlink)
            {   
                return View("CustomCheckout", payment);
            }
            else
            {
                return View("Checkout", payment);
            }
        }
       
        [HttpPost]
        public ActionResult DirectRequest(FormCollection collection)
        {
            PaymentVM pvm = (PaymentVM)TempData["payment"];
            bool isnew = pvm.SelectedAlias.isnew;
            SortedDictionary<string,string> param = Check.BuildQueryFromForm(collection,isnew);

            string tohash = Check.StringToHash(param, "Mypassphraseissocool!");
            string hash = Check.GetHash(tohash);
            param.Add("SHASIGN", hash);
            NameValueCollection query = Check.FromDicToCol(param);

            WebClient myWebClient = new WebClient();
            myWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            try
            {
                byte[] response = myWebClient.UploadValues("https://webdev.oglan.local/Ncol/Test/orderdirect.asp", query);
                string res = System.Text.Encoding.UTF8.GetString(response);
                XDocument document = XDocument.Parse(res);
                TempData["document"] = document;
                return RedirectToAction("EncodeDirectPayment", "PostSale");
            }
            catch(WebException e)
            {

            }
            return RedirectToAction("Index", "Home");
        }
        //[HttpPost]
        //public ActionResult UseAlias()
        //{
        //    OrderVM ordervm = (OrderVM)TempData["param"];
        //    if (String.IsNullOrEmpty(Request.Form["usealias"]))
        //    { ordervm.User.Alias = null; }
        //    else
        //    {
        //        if (Request.Form["alias"] == "")
        //        {
        //            TempData["param"] = ordervm;
        //            return View("CreateAlias", ordervm);
        //        }
        //        ordervm.SelectedAlias = AliasManager.FindAliasByName(Request.Form["alias"]);
        //    }
        //    TempData["param"] = ordervm;
        //    return RedirectToAction("CheckOut");
        //}
        //[HttpPost]
        //public ActionResult CreateAlias()
        //{
        //    OrderVM ordervm = (OrderVM)TempData["param"];
        //    if (!String.IsNullOrEmpty(Request.Form["createalias"]))
        //        ordervm.SelectedAlias = new Alias { Name = Request.Form["aliasvalue"] };
        //    else
        //        ordervm.SelectedAlias = new Alias();
        //    TempData["param"] = ordervm;
        //    return RedirectToAction("CheckOut");
        //}
        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
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

        // GET: Products/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Products/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Products/Delete/5
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
