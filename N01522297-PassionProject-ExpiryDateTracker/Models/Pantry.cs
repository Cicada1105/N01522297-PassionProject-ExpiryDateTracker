using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N01522297_PassionProject_ExpiryDateTracker.Models
{
    public class Pantry
    {
        [Key]
        public int PantryID { get; set; }
        public string PantryName { get; set; }

        // Foreign Key that references the User that owns the current Pantry
        [ForeignKey("User")]
        public int UserID { get; set; }
        // Reference the User that owns the current Pantry
        public virtual User User { get; set; }
    }
    public class PantryDto
    {
        public int PantryID { get; set; }
        public string PantryName { get; set; }
        public string UserFName { get; set; }
    }
}