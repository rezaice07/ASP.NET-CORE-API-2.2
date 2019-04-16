using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SamrtCRM.Data.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public string RoleName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
