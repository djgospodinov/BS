using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class EditAccountModel
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "Паролата трябва да е поне 6 символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Нова парола")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди нова парола")]
        [Compare("NewPassword", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}