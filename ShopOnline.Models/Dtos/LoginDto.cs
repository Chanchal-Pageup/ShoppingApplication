using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public class LoginDto
    {
        [MaxLength(50), MinLength(12), Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50), MinLength(8), Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

}
