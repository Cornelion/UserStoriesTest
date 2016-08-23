using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderVM
    {
        public List<ProductVM> Products { get; set; }
        public List<Alias> Aliases { get; set; }
        public Alias SelectedAlias { get; set; }
        public User User { get; set; }
        public SortedDictionary<string,string> Parameters {get;set;}
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (ProductVM prod in Products)
                {
                    total += prod.Total;
                }
                return total;
            }
         //   set { Total = value; }
        }
        public OrderVM()
        {
            Products = new List<ProductVM>();
        }
        public void AddParameter (string key, string value){
            Parameters.Add(key, value);
        }
        public void AddParameters()
        {
            SortedDictionary<string, string> Parameters = new SortedDictionary<string, string>();
            if (!String.IsNullOrEmpty(this.User.Alias))
            {
                Parameters.Add("ALIAS", this.User.Alias);
                Parameters.Add("ALIASUSAGE", "Click_here_to_use_alias");
            }
            Parameters.Add("AMOUNT", (this.Total * 100).ToString("0"));
            Parameters.Add("CURRENCY", "EUR");
            var orderId = DateTime.Now.ToString("ddhhMMss");
            Parameters.Add("ORDERID", orderId);
            Parameters.Add("PSPID", "EPhilippeTest");
        }
    }
}
