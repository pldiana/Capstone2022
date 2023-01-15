using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderResponse
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? DoneAt { get; set; }
        public decimal? Size { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }
        public string Side { get; set; }
        public string ProductId { get; set; }
        public decimal FilledSize { get; set; }

    }
}
