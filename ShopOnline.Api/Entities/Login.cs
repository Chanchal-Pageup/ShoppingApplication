using ApiBase.Contract.Entities;
using ShopOnline.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Api.Entities
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        public UserType UserTypeId { get; set; } = UserType.User;

        [Required]
        public int UserId { get; set; }

        [MaxLength(50), MinLength(12), Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
