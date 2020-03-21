using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Models
{
    public class Booking
    {
        public int BookingID { get; set; }

        public int BookID { get; set; }
        public virtual Book Book { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
