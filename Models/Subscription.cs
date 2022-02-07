using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Subscription
    {
        public DateTime? ExpirationDate { get; set; }
        public DateTime? LastRenewed { get; set; }
        public bool IsActive { get; set; }
    }
}
