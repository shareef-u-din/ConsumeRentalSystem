using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }
        public bool Availability { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; }

        public double UnitPrice { get; set; }
    }
}
