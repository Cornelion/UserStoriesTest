using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {  
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public List<Alias> Aliases { get; set; }
        public string Alias { get; set; }
    }
}
