using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_PassionProject_ExpiryDateTracker.Models.ViewModels
{
    public class DetailsUser
    {
        // Class for maintaining details for a user and the pantries that the owner has
        public User SelectedUser { get; set; }
        public IEnumerable<PantryDto> OwnedPantries { get; set; }
    }
}