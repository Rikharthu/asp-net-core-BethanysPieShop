using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        // Foreign Key for Orders
        public int OrderId { get; set; }
        public int PieId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public virtual Pie Pie { get; set; }
        public virtual Order Order { get; set; }
    }
}
