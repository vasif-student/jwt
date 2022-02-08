using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Models.Dtos
{
    public class StudentDto
    {
        [Required, MaxLength(10)]
        public string Name { get; set; }
        [Required, MaxLength(20)]
        public string Surname { get; set; }
    }
}
