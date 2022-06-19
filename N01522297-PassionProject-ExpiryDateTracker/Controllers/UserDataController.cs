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
    public class UserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Method for listing all users
        /// </summary>
        /// <example>
        /// GET: api/UserData/ListUsers
        /// </example>
        /// <returns>List of User objects</returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<User> ListUsers(string UserSearch = null)
        {
            if (UserSearch == null)
            {
                return db.CurrentUsers;
            }
            else
            {
                return db.CurrentUsers.Where(u => u.UserFName == UserSearch).ToList();
            }
        }

        /// <summary>
        /// Method for retrieving info about a specific user
        /// </summary>
        /// <param name="id">Integer value representing the unique id of the user</param>
        /// <example>
        /// GET: api/UserData/FindUser/2
        /// </example>
        /// <returns>User object of specified user id</returns>
        [ResponseType(typeof(User))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult FindUser(int id)
        {
            User user = db.CurrentUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Method for updating an exisiting user
        /// </summary>
        /// <param name="id">Integer value representing the user to be updated</param>
        /// <param name="user">User object containing info to be updated</param>
        /// <example>
        /// POST: api/UserData/UpdateUser/5
        /// </example>
        /// <returns>Redirects back to User list view</returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        /// Method for adding a newly created user to the database
        /// </summary>
        /// <param name="user">Newly created User object to be added to the database</param>
        /// <example>
        /// POST: api/UserData/AddUser
        /// </example>
        /// <returns>Redirects back to User list view</returns>
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CurrentUsers.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        /// <summary>
        /// Method for removing a specified user from the database
        /// </summary>
        /// <param name="id">Integer value representing the user to remove from the database</param>
        /// <example>
        /// POST: api/UserData/DeleteUser/5
        /// </example>
        /// <returns>Redirects back to User list view</returns>
        [ResponseType(typeof(User))]
        [HttpPost]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.CurrentUsers.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.CurrentUsers.Remove(user);
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

        private bool UserExists(int id)
        {
            return db.CurrentUsers.Count(e => e.UserID == id) > 0;
        }
    }
}