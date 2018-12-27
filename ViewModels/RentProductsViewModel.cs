using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class RentProductsViewModel
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalCost { get; set; }
        public bool Payment { get; set; }
        public bool Status { get; set; }

    }
}
