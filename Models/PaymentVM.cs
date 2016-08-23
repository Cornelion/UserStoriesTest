using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class PaymentVM
    {
        public OrderVM order;
        public User user;
        public bool isdirectlink { get; set; }
        public int amount { get; set; }
        public Alias SelectedAlias { get; set; }
        public SortedDictionary<string, string> Param { get; set; }
        //public string shasign { get; set; }
        public ActionResult next { get; set; }
        public PaymentVM()
        {
            this.Param = new SortedDictionary<string, string>();
        }
    }
}
