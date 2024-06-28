using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShopOnline.Models.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public class RegisterDto : EntityBase
    {
        [MaxLength(50), Required]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50), Required]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(50), EmailAddress, Required]
        public string Email { get; set; }

        [Required]
        public UserType UserTypeId { get; set; }

        public int? UserId { get; set; }
    }

    public class RegisterResponse
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

    }
}
