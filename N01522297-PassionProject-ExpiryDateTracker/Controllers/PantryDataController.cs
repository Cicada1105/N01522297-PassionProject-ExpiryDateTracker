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
using System.Diagnostics;

namespace N01522297_PassionProject_ExpiryDateTracker.Controllers
{
    public class PantryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Method for retrieving all pantris of all users
        /// </summary>
        /// <example>
        /// GET: api/PantryData/ListPantries
        /// </example>
        /// <returns>List of PantryDto objects containing information on all Pantries</returns>
        [HttpGet]
        //[Route("api/PantryData/ListPantries/{PantrySearch?}")]
        [Authorize]
        public IEnumerable<PantryDto> ListPantries(string PantrySearch = null)
        { 
            List<Pantry> Pantries;
            List<PantryDto> PantryDtos = new List<PantryDto>();

            if (PantrySearch != null)
            {
                Pantries = db.Pantries.Where(p => p.PantryName == PantrySearch).ToList();
            }
            else
            {
                Pantries = db.Pantries.ToList();
            }
            Pantries.ForEach(pantry => PantryDtos.Add(new PantryDto()
            {
                PantryID = pantry.PantryID,
                PantryName = pantry.PantryName,
                UserFName = pantry.User.UserFName
            }));

            return PantryDtos;
        }
        /// <summary>
        /// Method for retrieving the pantries owned by a specific user
        /// </summary>
        /// <param name="id">Integer value that represents the unique user to retrieve pantries for</param>
        /// <example>
        /// GET: api/PantryData/ListPantriesForUser/2
        /// </example>
        /// <returns>List of PantryDto objects containing information on all Pantries for the specified user</returns>
        [HttpGet]
        [Authorize]
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

        /// <summary>
        /// Method for retrieving information for a specific pantry
        /// </summary>
        /// <param name="id">Integer value for retrieving a specific pantry</param>
        /// <example>
        /// GET: api/PantryData/FindPantry/5
        /// </example>
        /// <returns>PantryDto object of specified pantry</returns>
        [ResponseType(typeof(Pantry))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult FindPantry(int id)
        {
            // Get the id of the current logged in user
            string userID = User.Identity.GetUserId();

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

        /// <summary>
        /// Method for updating an existing pantry
        /// </summary>
        /// <param name="id">Integer value of unique pantry to update</param>
        /// <param name="pantry">Pantry object containing newly updated info</param>
        /// <example>
        /// POST: api/PantryData/UpdatePantry/5
        /// </example>
        /// <returns>Redirects back the the Pantry list view</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Method for adding a new pantry to the database
        /// </summary>
        /// <param name="pantry">Newly created Pantry object</param>
        /// <example>
        /// POST: api/PantryData/AddPantry
        /// </example>
        /// <returns>Redirects baack to Pantry list view</returns>
        [ResponseType(typeof(Pantry))]
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Method for removing a specified pantry from the database
        /// </summary>
        /// <param name="id">Integer value representing the unique pantry to be removed</param>
        /// <example>
        /// POST: api/PantryData/DeletePantry/5
        /// </example>
        /// <returns>Redirects back to Pantry list view</returns>
        [ResponseType(typeof(Pantry))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeletePantry(int id)
        {
            Pantry pantry = db.Pantries.Where(p => p.PantryID == id).Single();
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