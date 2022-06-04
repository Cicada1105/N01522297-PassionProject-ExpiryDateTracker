using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace N01522297_PassionProject_ExpiryDateTracker.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserFName { get; set; } 
        public string UserLName { get; set; }
    }
}