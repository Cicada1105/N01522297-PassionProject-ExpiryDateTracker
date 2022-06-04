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

namespace N01522297_PassionProject_ExpiryDateTracker.Controllers
{
    public class ItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ItemData/ListItems
        [HttpGet]
        public IEnumerable<ItemDto> ListItems()
        {
            List<Item> Items = db.Items.ToList();
            List<ItemDto> ItemDtos = new List<ItemDto>();

            Items.ForEach(item => ItemDtos.Add(new ItemDto(){
                ItemID = item.ItemID,
                ItemName = item.ItemName,
                ItemExpiry = item.ItemExpiry,
                PantryName = item.Pantry.PantryName
            }));

            return ItemDtos;
        }
        // GET: api/ItemData/ListItemsForPantry/
        [HttpGet]
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

        // GET: api/ItemData/FindItem/5
        [ResponseType(typeof(Item))]
        [HttpGet]
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

        // POST: api/ItemData/UpdateItem/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/ItemData/AddItem
        [ResponseType(typeof(Item))]
        [HttpPost]
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

        // POST: api/ItemData/DeleteItem/5
        [ResponseType(typeof(Item))]
        [HttpPost]
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