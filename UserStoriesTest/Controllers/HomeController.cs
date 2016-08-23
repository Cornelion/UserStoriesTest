using Helpers.DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UserStoriesTest.Controllers
{
    public class HomeController : Controller
    {
        public List<Product> productsList;
        public ActionResult Index()
        {
            var session = Session["Order"];
            if (session == null)
            {
                Session["Order"] = new Order();
            }
            return View(productsList);
        }
        public HomeController()
        {   
               // Session["Order"] = new Order();
            
            productsList = Catalog.GetProducts();
        }
    }
}