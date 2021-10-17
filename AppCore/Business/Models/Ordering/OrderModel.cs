using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Business.Models.Ordering
{
   public class OrderModel
    {
        public string Expression { get; set; }
        public bool DirectionAscending { get; set; } = true;
    }
}
