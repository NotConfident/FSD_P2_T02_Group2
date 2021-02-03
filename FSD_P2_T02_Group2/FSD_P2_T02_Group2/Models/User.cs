using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FSD_P2_T02_Group2.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage ="Username must be provided.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password must be provided.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number must be provided.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Email must be provided.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name must be provided.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Alias must be provided.")]
        public string Alias { get; set; }

        public string Status { get; set; } = "";

        public string Image { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public List<PostViewModel> PostList { get; set; }
    }
}
