using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsTableForWho.Models
{
    public class User
    {

        [Key]
        public int UserId { get; set; }


        [Required(ErrorMessage = "...is required."), MinLength(2, ErrorMessage = "...requires a min of {1} characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "...is required.")]
        [Display(Name = "Table For")]
        public int TableFor { get; set; } = 0;      // default for Admin User only


        [Required(ErrorMessage = "...is required."), RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$", ErrorMessage = "...must match format ###-###-####.")]
        // [Required(ErrorMessage = "...is required.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = "000-000-0000";       // default for Admin User only - Not Null - DB constraint
                                                                        // also cannot be "EMPLOYEE" - RegEx prevents it - also ugly design to have EMPLOYEE in PhoneNumber field!



        public bool isSeated { get; set; }
        public bool hasLeft { get; set; }




        [Display(Name = "Role")]
        public int RoleId { get; set; } = 2;     // 1 = Admin  // 2 = Customer


        [Required(ErrorMessage = "...is required."), MinLength(8, ErrorMessage = "...requires a min of {1} characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "00000000";      // default for Customer User only


        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPswd { get; set; } = "00000000";   // default for Customer User only




        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }

}