using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public class Order
    {   public List<Product> Products { get; set; }
        public decimal Total
        {
            get {
                decimal total = 0;
                foreach (Product product in Products )
                {
                    total += product.Price;
                }
                return total;
                }
            set { Total = value; }
        }
        public Order()
        {
            Products = new List<Product>();
        }
        public void RemoveItem(int id)
        {
            
            if (Products.Exists(x => x.Ref == id))
            {
                Product ToRemove = Products.Where(p => p.Ref == id).FirstOrDefault() ;
                Products.Remove(ToRemove);
            }
        }
    }
}
