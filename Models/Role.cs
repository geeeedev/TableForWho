using System;
using System.ComponentModel.DataAnnotations;

namespace CsTableForWho.Models
{
    public class Role
    {
        
        [Key]
        public int RoleId { get; set; }     // 1 = Admin  // 2 = Customer
        public string RoleName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

}