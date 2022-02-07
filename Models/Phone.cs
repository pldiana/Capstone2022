using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Phone
    {
        public int Number { get; set; }
        public PhoneType TypeOfPhone { get; set; }
        public bool? IsPrimary { get; set; }
    }
}
