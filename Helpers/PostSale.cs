using Helpers.DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class PostSale
    {
        public bool VerifyParams(NameValueCollection stringparam)
        {
            if (!String.IsNullOrEmpty(stringparam["ALIAS"]))
            {
                //HasUserAlias(stringparam["ALIAS"], stringparam["CN"]);
                if (!AliasManager.CheckifAliasExist(stringparam["ALIAS"]))
                {
                    AliasManager.Add(AliasManager.Create(
                       stringparam["ALIAS"],
                       stringparam["CN"],
                       stringparam["ED"],
                       stringparam["CARDNO"]
                       ));

                }
            }


            StringBuilder sbuilder = new StringBuilder();
            string passphrase = "Thisisa16sha-out";
            string shaout="";
            
            foreach (string key in stringparam.AllKeys.OrderBy(k => k))
            {
                if (key != "SHASIGN")
                {
                    sbuilder.Append(key.ToUpper() + "=" + stringparam[key] + passphrase);
                }
                else
                    shaout = stringparam[key];
            }
           
           
            if (shaout.Equals(Crypto.GetSHA256(sbuilder.ToString())))
                return true;
            else
                return false;
        }
       
        public void HasUserAlias(string alias,string username)
        {
            Registry registry = new DAL.Registry();
            User user = registry.FindUserByName(username);
            registry.UpdateUserAlias(user,alias);
        }
        public string[] FindIdAndStatus(NameValueCollection stringparam)
        {
            string[] values = new string[2];
            values[0] = stringparam["orderID"];
            values[1] = stringparam["STATUS"];
            return values;
        }
        public void LineChanger(Payment payment, int line_to_edit)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/PaymentsList.txt");
            string[] arrLine = File.ReadAllLines(path);
            arrLine[line_to_edit] = 
                payment.PayId + ";" +
                payment.CustomerName + ";" +
                payment.Orderid + ";" +
                payment.Currency + ";" +
                payment.Amount + ";" +
                payment.Status ;
            File.WriteAllLines(path, arrLine);
        }
        //param.Add("AMOUNT", (Convert.ToDecimal(payment.Amount) * 100).ToString("0.##"));
        public List<string> GenerateBatch(List<Payment> payments)
        {
            string numberofpayments = payments.Count().ToString();
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/BatchFile.txt");
            List<string> order = new List<string>();
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            string ohlheader = "OHL;EPhilippeTest;N4t&1ytqE#;;user-API;";
            order.Add(EncodeLine(ohlheader));
            string header = "OHF;File;MTR;SAS;" + numberofpayments + ";";
            order.Add(EncodeLine(header));
            foreach (Payment payment in payments)
            {
                string line =
                    ((Convert.ToDecimal(payment.Amount) * 100).ToString("0.##")) + ";" +
                    payment.Currency + ";" +
                    ";" +
                    ";" +
                    ";" +
                    payment.Orderid + ";" +
                    ";" +
                    ";" +
                    payment.PayId + ";" +
                    "SAS;";
               
                
                order.Add(EncodeLine(line));
            }
            order.Add(EncodeLine("OTF;"));
            return order;
           //File.Create(path).Close();
           //File.AppendAllText(path, order.ToString());
            
            
        }
        public string EncodeLine(string source)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            List<byte> bytes = asciiEncoding.GetBytes(source).ToList();
            bytes.Add(0xd);
            bytes.Add(0xA);
            return asciiEncoding.GetString(bytes.ToArray());
        }
    }
}
