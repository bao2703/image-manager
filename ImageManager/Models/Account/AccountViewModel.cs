using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.Models.Account
{
    public class AccountViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}