﻿using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}