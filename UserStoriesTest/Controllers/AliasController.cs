using Helpers.DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserStoriesTest.Controllers
{
    public class AliasController : Controller
    {
        // GET: Alias
        public ActionResult Index()
        {
            List<Alias> aliases = (List<Alias>)TempData["aliases"] ?? new List<Alias>();
            if (aliases.Count > 0)
            {
                List<SelectListItem> items = new List<SelectListItem>();
                foreach (Alias alias in aliases)
                {
                    items.Add(new SelectListItem { Text = alias.Name + alias.CardNo, Value = alias.Name });
                }
                items.Add(new SelectListItem { Text = "Use a new card", Value = "" });
                ViewBag.alias = items;
                //TempData["param"] = final;
                return View("AliasPage");
            }
            else
            {
                //TempData["param"] = final;
                return View("CreateAlias");
            }
            
        }
        public ActionResult UseAlias()
        {
            Alias alias = new Alias();
            if (String.IsNullOrEmpty(Request.Form["usealias"]))
            { TempData["selectedalias"] = new Alias() { isnew = true }; }
            else
            {
                if (Request.Form["alias"] == "")
                {  
                    return View("CreateAlias");
                }  
                alias = AliasManager.FindAliasByName(Request.Form["alias"]);
                alias.isnew = false;
                TempData["selectedalias"] = alias;
            }
            return RedirectToAction("CheckOut","Products");
        }
        [HttpPost]
        public ActionResult CreateAlias()
        {
            Alias alias = new Alias();
            if (!String.IsNullOrEmpty(Request.Form["createalias"]))
                alias = new Alias { Name = Request.Form["aliasvalue"] , isnew=true};
            else
                alias = new Alias() { isnew = true };
            TempData["selectedalias"] = alias;
            return RedirectToAction("CheckOut","Products");
        }
    }
}