using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using N01522297_PassionProject_ExpiryDateTracker.Models;
using N01522297_PassionProject_ExpiryDateTracker.Models.ViewModels;

namespace N01522297_PassionProject_ExpiryDateTracker.Controllers
{
    public class UserController : Controller
    {
        // Create a static instance of the http client connection to allow reuse
        private static readonly HttpClient client;
        // Create new javascript serializer
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
        
        static UserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44332/api/");
        }
        // GET: User/List
        public ActionResult List()
        {
            // Define url path that the ListUsers API Endpoint
            string url = "UserData/ListUsers";

            // Send request to API
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Retrieve the list of users
            IEnumerable<User> users = resp.Content.ReadAsAsync<IEnumerable<User>>().Result;

            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            // Create a new instance of the DetailsUser ViewModel to store user details and the pantries they own
            DetailsUser userVM = new DetailsUser();

            // Define the url path to the corresponding API Endpoint for the FindUser
            string url = $"UserData/FindUser/{id}";
            // Make request to the API, storing resulting response
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Store the resutling User object
            User retrievedUser = resp.Content.ReadAsAsync<User>().Result;
            // Store retrievedUser in the ViewModel
            userVM.SelectedUser = retrievedUser;

            // Define url path that the ListPantries API Endpoint
            url = $"PantryData/ListPantriesForUser/{id}";
            // Send request to API
            resp = client.GetAsync(url).Result;
            // Retrieve the list of pantries
            IEnumerable<PantryDto> pantries = resp.Content.ReadAsAsync<IEnumerable<PantryDto>>().Result;
            // Store user owned pantries in ViewModel
            userVM.OwnedPantries = pantries;

            // Send the ViewModel to the view
            return View(userVM);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: User/New
        public ActionResult New()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User newUser)
        {
            // Define url path to the AddUser API Endpoint
            string url = "UserData/AddUser";

            // Serialize the newly created User object
            string payload = serializer.Serialize(newUser);
            // Convert the json serialized string into an HTTP readable object
            HttpContent content = new StringContent(payload);
            // Define the content-type header to let the api know incoming data is of type applicaiton/json
            content.Headers.ContentType.MediaType = "application/json";
            // Send post request, storing the resulting response
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            // Check if response was successful
            if (resp.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            // Define the url path to the corresponding API Endpoint for the FindUser
            string url = $"UserData/FindUser/{id}";

            // Make request to the API, storing resulting response
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Store the resutling User object
            User retrievedUser = resp.Content.ReadAsAsync<User>().Result;

            // Send the newly found User to the view
            return View(retrievedUser);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, User updatedUser)
        {
            // Define url path associated with the API Endpoint for updating a user
            string url = $"UserData/UpdateUser/{id}";

            // Convert the updated user into a json string
            string payload = serializer.Serialize(updatedUser);
            // Format the payload into a HTTP Request readable object
            HttpContent content = new StringContent(payload);
            // Set header type to reflect the json data
            content.Headers.ContentType.MediaType = "application/json";
            // Make post request, storing returned response
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            // Check if response was successful
            if (resp.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: User/Remove/5
        public ActionResult Remove(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // Define url path to the API Endpoint for deleting a user
            string url = $"UserData/DeleteUser/{id}";
            // Format post data to be HTTP readable
            HttpContent content = new StringContent("");
            // Send post request, including empty content and storing response
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            // Check if response was successful
            if (resp.IsSuccessStatusCode)
            {
                // TODO: Add delete logic here

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
