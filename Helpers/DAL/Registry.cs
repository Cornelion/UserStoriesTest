using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Helpers.DAL
{
    public class Registry
    {
        public List<User> GetAllUsers()
        {
            int x = 0;
            string [] users = System.IO.File.ReadAllLines(System.Web.HttpContext.Current.Server.MapPath("~/Content/UsersList.txt"));
            return users.Select(product => product.Split(new string[] { ";" }, StringSplitOptions.None))
            .Select(details => new User
            {
                UserName = details[0],
                PassWord = details[1],
                //Alias= details[2]
                
            })
            .ToList();
        }
        public  User FindActiveUser(HttpCookie authCookie)
        {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                return FindUserByName(authTicket.Name);
        }
        public  User FindUserByName(string name)
        {
            return GetAllUsers().Where(u => u.UserName == name).FirstOrDefault();
        }
        public void UpdateUserAlias(User user,string alias)
        {
            List<User> users = GetAllUsers();
            int index = users.FindIndex(u => u.UserName == user.UserName);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/UsersList.txt");
            string[] arrLine = File.ReadAllLines(path);
            arrLine[index] =
                user.UserName + ";" +
                user.PassWord + ";" +
                alias;
            File.WriteAllLines(path, arrLine);
        }
        public bool CheckIfUserExist(User user)
        {
            return GetAllUsers().Exists(u => u.PassWord == user.PassWord);
        }
    }
}
