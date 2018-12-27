using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int PaymentId { get; set; }
        public bool Valid { get; set; }
        public string Photo { get; set; }
    }
}
