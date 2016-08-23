using Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.DAL
{
    public class Ledger
    {   
        public static void Add(NameValueCollection stringparam)
        {
            string[] line = new string[] { PreparePayment(stringparam) };
            System.IO.File.AppendAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/PaymentsList.txt"), line);
        }

        public static void Update(NameValueCollection stringparam)
        {

        }
        public static string PreparePayment(NameValueCollection stringparam)
        {
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append(
                stringparam["PAYID"] + ";" +
                stringparam["CN"] + ";" +
                stringparam["orderID"] + ";" +
                stringparam["currency"] + ";" +
                stringparam["amount"] + ";" +
                stringparam["STATUS"] 
                );
            return sbuilder.ToString();
        }
        public static Payment FindPaymentByPayid(int reference)
        {
            return GetAllPayments().Where(p => p.PayId == reference.ToString()).FirstOrDefault();
        }
        public static List<Payment> GetAllPayments()
        {
            string[] users = System.IO.File.ReadAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/PaymentsList.txt"));
            return users.Select(product => product.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            .Select(details => new Payment
            {
               PayId          = details[0],
               CustomerName   = details[1],
               Orderid        = details[2],
               Currency       = details[3],
               Amount         = details[4],
               Status         = details[5]
            })
            .ToList();
        }
    }
}
