using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace biblioteca.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public string Name { get; set; }

        public string Cpf { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
    }
}
