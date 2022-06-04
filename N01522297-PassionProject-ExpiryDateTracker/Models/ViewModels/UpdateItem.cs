using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_PassionProject_ExpiryDateTracker.Models.ViewModels
{
    public class UpdateItem
    {
        // Class for handling need for retrieving an item details to be updated and a list of pantries for the user to select from

        // Item details to be update
        public ItemDto SelectedItem { get; set; }

        // List of pantries the user can select from when updating
        public IEnumerable<PantryDto> PantryOptions { get; set; }
    }
}