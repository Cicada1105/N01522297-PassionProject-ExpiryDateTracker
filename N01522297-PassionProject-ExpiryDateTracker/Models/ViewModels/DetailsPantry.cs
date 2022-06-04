using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_PassionProject_ExpiryDateTracker.Models.ViewModels
{
    public class DetailsPantry
    {
        // Class for maintaining details for a pantry and all of the items stored in that pantry
        public PantryDto SelectedPantry { get; set; }
        public IEnumerable<ItemDto> StoredItems { get; set; }
    }
}