using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Helpers.DAL
{
    public class Catalog
    {   public static List<Product> GetProducts()
        {
            string[] products =
                   System.IO.File.ReadAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/ProductsList.txt"));
            return products.Select(product => product.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            .Select(detailedProduct => new Product
            {
                Ref = Int32.Parse(detailedProduct[0]),
                Name = detailedProduct[1],
                Price = Decimal.Parse(detailedProduct[2])
            })
            .ToList();

        }
        public static Product CloneProduct(int id)
        {
            Product Original = GetProducts().Where(product => product.Ref == id).FirstOrDefault();
           return new Product
            {
                Ref = Original.Ref,
                Name = Original.Name,
                Price = Original.Price
            };
        }
    }
}
