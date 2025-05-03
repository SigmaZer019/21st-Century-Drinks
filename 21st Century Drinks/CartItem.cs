using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21st_Century_Drinks
{
    internal class CartItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public double TotalPrice => Quantity * UnitPrice;
    }

}
