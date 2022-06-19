using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using N01522297_PassionProject_ExpiryDateTracker.Models;
using Microsoft.AspNet.Identity;

namespace N01522297_PassionProject_ExpiryDateTracker.Controllers
{
    public class ItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Method for returning a List of Items created by all users
        /// </summary>
        /// <example>
        /// GET api/ItemData/ListItems
        /// </example>
        /// <returns>List of ItemDto objects containing info about items owned by users</returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<ItemDto> ListItems(string ItemSearch = null)
        {
            List<Item> Items;
            List<ItemDto> ItemDtos = new List<ItemDto>();

            if (ItemSearch != null)
            {
                Items = db.Items.Where(i => i.ItemName == ItemSearch).ToList();
            }
            else
            {
                Items = db.Items.ToList();
            }

            Items.ForEach(item => ItemDtos.Add(new ItemDto(){
                ItemID = item.ItemID,
                ItemName = item.ItemName,
                ItemExpiry = item.ItemExpiry,
                PantryName = item.Pantry.PantryName
            }));

            return ItemDtos;
        }
        /// <summary>
        /// Method for returning all items that belong to a specific pantry
        /// </summary>
        /// <param name="id">Integer value representing the id of a pantry</param>
        /// <example>
        /// GET api/ItemData/ListItemsForPantry/2
        /// </example>
        /// <returns>List of ItemDto objects containing info of items the belong to the corresponding pantry</returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<ItemDto> ListItemsForPantry(int id)
        {
            List<Item> Items = db.Items.Where(item => item.PantryID == id).ToList();
            List<ItemDto> ItemDtos = new List<ItemDto>();

            Items.ForEach(item => ItemDtos.Add(new ItemDto()
            {
                ItemID = item.ItemID,
                ItemName = item.ItemName,
                ItemExpiry = item.ItemExpiry,
                PantryName = item.Pantry.PantryName
            }));

            return ItemDtos;
        }

        /// <summary>
        /// Method for finding the specific details of an item
        /// </summary>
        /// <param name="id">Integer value that represents the unique item to retrieve</param>
        /// <example>
        /// GET api/ItemData/FindItem/5
        /// </example>
        /// <returns>ItemDto object containing info about the specified item</returns>
        [ResponseType(typeof(Item))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult FindItem(int id)
        {
            Item Item = db.Items.Find(id);
            ItemDto ItemDto = new ItemDto()
            {
                ItemID = Item.ItemID,
                ItemName = Item.ItemName,
                ItemExpiry = Item.ItemExpiry,
                PantryName = Item.Pantry.PantryName
            };
            if (Item == null)
            {
                return NotFound();
            }

            return Ok(ItemDto);
        }

        /// <summary>
        /// Method for updating an existing item
        /// </summary>
        /// <param name="id">Integer value of the unique item to update</param>
        /// <param name="item">Item object containing newly updated info</param>
        /// <example>
        /// POST: api/ItemData/UpdateItem/5
        /// </example>
        /// <returns>Redirects back to list view of items</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ItemID)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Method for adding a new item to the database
        /// </summary>
        /// <param name="item">Newly created Item object to be added to the database</param>
        /// <example>
        /// POST: api/ItemData/AddItem
        /// </example>
        /// <returns>Redirects back to List view of items</returns>
        [ResponseType(typeof(Item))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.ItemID }, item);
        }

        /// <summary>
        /// Method for removing a specified item from the database
        /// </summary>
        /// <param name="id">Integer value representing the unique item to be removed</param>
        /// <example>
        /// POST: api/ItemData/DeleteItem/5
        /// </example>
        /// <returns>Redirects back to the List view of items</returns>
        [ResponseType(typeof(Item))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.ItemID == id) > 0;
        }
    }
}