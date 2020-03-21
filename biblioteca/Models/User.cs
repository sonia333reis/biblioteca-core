using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string Name { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
