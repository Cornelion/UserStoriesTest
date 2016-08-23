using Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Helpers
{
    public class Check
    {
        public static OrderVM GenerateOrderVM(Order Order)
        {
            var order = Order.Products
                 .GroupBy(o => o.Name)
                 .Select(p => new ProductVM
                 {
                     Name = p.Key,
                     Quantity = p.Count(),
                     Total = p.Sum(x => x.Price)
                 }).ToList();
            OrderVM OrderVM = new OrderVM();
            OrderVM.Products.AddRange(order);

            return OrderVM;
        }
        public static void SetParameters(PaymentVM payment)
        {
            if (payment.SelectedAlias != null && !String.IsNullOrEmpty(payment.SelectedAlias.Name))
            {
                payment.Param.Add("ALIAS", payment.SelectedAlias.Name);
                payment.Param.Add("ALIASUSAGE", "Click_here_to_use_alias");
                //if (!payment.isdirectlink)
                //{
                //    payment.Param.Add("CARDNO", payment.SelectedAlias.CardNo);
                //    payment.Param.Add("ED", payment.SelectedAlias.Ed);
                //}
            }
            payment.Param.Add("AMOUNT", (payment.order.Total * 100).ToString("0"));
           
          
            payment.Param.Add("CURRENCY", "EUR");
            var orderId = DateTime.Now.ToString("ddhhMMss");
            payment.Param.Add("OPERATION", "RES");
            payment.Param.Add("ORDERID", orderId);
            payment.Param.Add("PSPID", "EPhilippeTest");
            if (payment.isdirectlink)
            {
                payment.Param.Add("PSWD", "N4t&1ytqE#");
                payment.Param.Add("USERID", "user-API");
                
            }
            else
            {
                payment.Param.Add("CN", payment.user.UserName);
                string sha = GetHash(StringToHash(payment.Param, "Mypassphraseissocool!"));
                payment.Param.Add("SHASIGN", sha);
            }
            
        }
        //public static SortedDictionary<string,string> GetParameters(OrderVM payment)
        //{
        //    SortedDictionary<string, string> Parameters = new SortedDictionary<string, string>();
        //    if (payment.SelectedAlias!=null && !String.IsNullOrEmpty(payment.SelectedAlias.Name))
        //    {
        //        Parameters.Add("ALIAS", payment.SelectedAlias.Name);
        //        Parameters.Add("ALIASUSAGE", "Click_here_to_use_alias");
        //        //Parameters.Add("CARDNO", payment.SelectedAlias.CardNo);
        //        //Parameters.Add("ED", payment.SelectedAlias.Ed);
        //    }
        //    Parameters.Add("AMOUNT", (payment.Total * 100).ToString("0"));

        //    Parameters.Add("CN", payment.User.UserName);
        //    Parameters.Add("CURRENCY", "EUR");
        //    var orderId = DateTime.Now.ToString("ddhhMMss");
        //    Parameters.Add("OPERATION", "RES");
        //    Parameters.Add("ORDERID", orderId);
        //    Parameters.Add("PSPID", "EPhilippeTest");
        //    Parameters.Add("PSWD", "101Pass");
        //    Parameters.Add("USERID", "API-user");
        //    // Parameters.Add("TP", @"https://webdev.oglan.local/Users/per/Integration/Content/CustomCheckOut.html");
        //    return Parameters;
        //}
        public static string StringToHash(SortedDictionary<string,string> parameters,string pass)
        {
            StringBuilder ToHash = new StringBuilder();
            foreach(KeyValuePair<string,string> KV in parameters)
            {
                ToHash.Append(KV.Key + "=" + KV.Value + pass);
            }
            return ToHash.ToString();
        }
        public static string GetHash(string toHash)
        {
            return Crypto.GetSHA256(toHash);
        }
        public static SortedDictionary<string, string> BuildQueryFromForm(FormCollection collection,bool isnew)
        {
            SortedDictionary<string,string> dico = new SortedDictionary<string, string>();
            foreach(string paramName in collection.AllKeys)
            {   if(paramName=="CN" || paramName=="ED" || paramName=="CARDNO")
                { if (isnew) dico.Add(paramName, collection[paramName]);}
                else if(paramName=="BRAND")
                { dico.Add(paramName, collection[paramName]);dico.Add("PM", "CreditCard"); }
                else
                dico.Add(paramName, collection[paramName]);
            };
            return dico;
        }
        public static NameValueCollection FromDicToCol(SortedDictionary<string, string> dico)
        {
            NameValueCollection query = new NameValueCollection();
            foreach (string st in dico.Keys)
            {
                query.Add(st, dico[st]);
            };
            return query;
        }

       
    }
}