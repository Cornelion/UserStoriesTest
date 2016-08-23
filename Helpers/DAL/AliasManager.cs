using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.DAL
{
    public static class AliasManager
    {
        public static List<Alias> FindAliasesForUser(string name)
        {
            return GetAll().Where(a => a.Cn == name).ToList();
        }
        public static Alias FindAliasByName(string name)
        {
            return GetAll().Where(a => a.Name == name).FirstOrDefault();
        }
        public static List<Alias> GetAll()
        {
            string[] users = System.IO.File.ReadAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/Aliases.txt"));
            if (users.Count() > 0)
                return users.Select(alias => alias.Split(new string[] { ";" }, StringSplitOptions.None))
                .Select(details => new Alias
                {
                    Name = details[0],
                    Cn = details[1],
                    Ed = details[2],
                    CardNo = details[3]
                })
                .ToList();
            else
                return new List<Alias>();
        }
        public static void Add(Alias alias)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(alias.Name + ";" + alias.Cn + ";" + alias.Ed + ";" + alias.CardNo);
            string[] line = new string[] { sb.ToString() };
            System.IO.File.AppendAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/Aliases.txt"), line);
        }
        public static Alias Create(string alias, string cn,string ed, string cardno)
        {
            return new Alias { Name = alias, Cn = cn, Ed = ed, CardNo = cardno };

        }
        public static bool CheckifAliasExist(string alias)
        {

            return GetAll().Exists(a => a.Name == alias);
        }
    }
}
