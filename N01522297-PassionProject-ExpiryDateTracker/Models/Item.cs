using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N01522297_PassionProject_ExpiryDateTracker.Models
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public DateTime ItemExpiry { get; set; }

        // Foreign Key that references the Pantry the current Item belongs to
        // An Item belongs to a Pantry and a Pantry can have many Items
        [ForeignKey("Pantry")]
        public int PantryID { get; set; }
        public virtual Pantry Pantry { get; set; }
    }

    public class ItemDto
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public DateTime ItemExpiry { get; set; }
        public string PantryName { get; set; }
    }
}