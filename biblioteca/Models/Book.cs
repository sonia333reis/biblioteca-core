using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        public string Name { get; set; }

        public string Writter { get; set; }

        public DateTime Release { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
