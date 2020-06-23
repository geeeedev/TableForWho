using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsTableForWho.Models
{
    [NotMapped]
    public class LoginUser
    {
        [Display(Name="Username")]
        [Required(ErrorMessage="...Missing!")]
        public string LoginName { get; set; }   

        [Display(Name="Password")]
        [Required(ErrorMessage="...Missing!"),]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}
