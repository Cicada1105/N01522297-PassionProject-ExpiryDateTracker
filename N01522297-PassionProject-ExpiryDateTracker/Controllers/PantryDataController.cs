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
    public class PantryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PantryData/ListPantries
        [HttpGet]
        public IEnumerable<PantryDto> ListPantries()
        {
            List<Pantry> Pantries = db.Pantries.ToList();
            List<PantryDto> PantryDtos = new List<PantryDto>();

            Pantries.ForEach(pantry => PantryDtos.Add(new PantryDto()
            {
                PantryID = pantry.PantryID,
                PantryName = pantry.PantryName,
                UserFName = pantry.User.UserFName
            }));

            return PantryDtos;
        }
        // GET: api/PantryData/ListPantriesForUser
        [HttpGet]
        public IEnumerable<PantryDto> ListPantriesForUser(int id)
        {
            List<Pantry> Pantries = db.Pantries.Where(pantry => pantry.UserID == id).ToList();
            List<PantryDto> PantryDtos = new List<PantryDto>();

            Pantries.ForEach(pantry => PantryDtos.Add(new PantryDto()
            {
                PantryID = pantry.PantryID,
                PantryName = pantry.PantryName,
                UserFName = pantry.User.UserFName
            }));

            return PantryDtos;
        }

        // GET: api/PantryData/FindPantry/5
        [ResponseType(typeof(Pantry))]
        [HttpGet]
        public IHttpActionResult FindPantry(int id)
        {
            Pantry Pantry = db.Pantries.Find(id);
            PantryDto PantryDto = new PantryDto()
            {
                PantryID = Pantry.PantryID,
                PantryName = Pantry.PantryName,
                UserFName = Pantry.User.UserFName
            };
            if (Pantry == null)
            {
                return NotFound();
            }

            return Ok(PantryDto);
        }

        // POST: api/PantryData/UpdatePantry/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePantry(int id, Pantry pantry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pantry.PantryID)
            {
                return BadRequest();
            }

            db.Entry(pantry).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PantryExists(id))
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

        // POST: api/PantryData/AddPantry
        [ResponseType(typeof(Pantry))]
        [HttpPost]
        public IHttpActionResult AddPantry(Pantry pantry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pantries.Add(pantry);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pantry.PantryID }, pantry);
        }

        // POST: api/PantryData/DeletePantry/5
        [ResponseType(typeof(Pantry))]
        [HttpPost]
        public IHttpActionResult DeletePantry(int id)
        {
            Pantry pantry = db.Pantries.Find(id);
            if (pantry == null)
            {
                return NotFound();
            }

            db.Pantries.Remove(pantry);
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

        private bool PantryExists(int id)
        {
            return db.Pantries.Count(e => e.PantryID == id) > 0;
        }
    }
}