using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_PassionProject_ExpiryDateTracker.Models.ViewModels
{
    public class UpdatePantry
    {
        // Class for handling retrieval of pantry to be updated and list of users to be selected from as the owner of the updated pantry

        // Pantry details to be update
        public PantryDto SelectedPantry { get; set; }

        // List of users to be selected from as the owner of the update pantry\
        public IEnumerable<User> UserOptions { get; set; }
    }
}