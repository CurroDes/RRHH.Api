using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.DTOs
{
    public class AuthApiViewModelDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
