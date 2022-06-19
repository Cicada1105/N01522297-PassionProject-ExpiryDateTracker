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
    public class PantryController : Controller
    {
        // Create a static instance of the http client connection to allow reuse
        private static readonly HttpClient client;
        // Create new javascript serializer
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static PantryController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44332/api/");
        }
        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        // GET: Pantry/List
        [Authorize]
        public ActionResult List(string PantrySearch = null)
        {
            GetApplicationCookie();
            // Define url path that the ListPantries API Endpoint
            string url = "PantryData/ListPantries";

            if (PantrySearch != null)
            {
                url += "?PantrySearch=" + PantrySearch;
            }

            // Send request to API
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Retrieve the list of pantries
            IEnumerable<PantryDto> pantries = resp.Content.ReadAsAsync<IEnumerable<PantryDto>>().Result;

            // Pass the resulting pantries to the view
            return View(pantries);
        }

        // GET: Pantry/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            // Create a DetailsPantry ViewModel for storing retrieved pantry details and the items belonging to that pantry
            DetailsPantry pantryVM = new DetailsPantry();

            // Define the url path to the corresponding API Endpoint for the FindPantry
            string url = $"PantryData/FindPantry/{id}";
            // Make request to the API, storing resulting response
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Store the resutling PantryDto object
            PantryDto retrievedPantry = resp.Content.ReadAsAsync<PantryDto>().Result;
            // Store the retrieved pantry in the ViewModel
            pantryVM.SelectedPantry = retrievedPantry;

            // Retrieve items belonging to the current pantry
            url = $"ItemData/ListItemsForPantry/{id}";
            // Make request to the API, storing resulting response
            resp = client.GetAsync(url).Result;
            IEnumerable<ItemDto> storedItems = resp.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;
            // Store retrieved items in the ViewModel
            pantryVM.StoredItems = storedItems;

            // Send the newly found PantryDto to the view
            return View(pantryVM);
        }
        [Authorize]
        public ActionResult Error()
        {
            return View();
        }

        // GET: Pantry/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            // Retrieve the Users to allow the user to select from one
            string url = "UserData/ListUsers";

            // Send request to API
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Retrieve the list of users
            IEnumerable<User> users = resp.Content.ReadAsAsync<IEnumerable<User>>().Result;

            // Pass in retrieved users to the view
            return View(users);
        }

        // POST: Pantry/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Pantry newPantry)
        {
            GetApplicationCookie();
            // Define url path to the AddPantry API Endpoint
            string url = "PantryData/AddPantry";

            // Serialize the newly created Pantry object
            string payload = serializer.Serialize(newPantry);
            // Convert the json serialized string into an HTTP readable object
            HttpContent content = new StringContent(payload);
            // Define the content-type header to let the api know incoming data is of type applicaiton/json
            content.Headers.ContentType.MediaType = "application/json";
            // Send post request, storing the resulting response
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            // Check if response was successful
            if(resp.IsSuccessStatusCode) {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pantry/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            // Create a new instance of the UpdatePantry ViewModel
            UpdatePantry pantryVM = new UpdatePantry();

            // Define the url path to the corresponding API Endpoint for the FindPantry
            string url = $"PantryData/FindPantry/{id}";
            // Make request to the API, storing resulting response
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Store the resutling PantryDto object
            PantryDto retrievedPantry = resp.Content.ReadAsAsync<PantryDto>().Result;
            // Store the retrieved pantry info in the ViewModel
            pantryVM.SelectedPantry = retrievedPantry;

            // Define url path that the ListUsers API Endpoint
            url = "UserData/ListUsers";
            // Send request to API
            resp = client.GetAsync(url).Result;
            // Retrieve the list of users
            IEnumerable<User> users = resp.Content.ReadAsAsync<IEnumerable<User>>().Result;
            // Add the retrieved users to the ViewModel
            pantryVM.UserOptions = users;

            // Send the pantry ViewModel to the view
            return View(pantryVM);
        }

        // POST: Pantry/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Pantry updatedPantry)
        {
            GetApplicationCookie();
            // Define url path associated with the API Endpoint for updating a pantry
            string url = $"PantryData/UpdatePantry/{id}";

            // Convert the updated pantry into a json string
            string payload = serializer.Serialize(updatedPantry);
            // Format the payload into a HTTP Request readable object
            HttpContent content = new StringContent(payload);
            // Set header type to reflect the json data
            content.Headers.ContentType.MediaType = "application/json";
            // Make post request, storing returned response
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            // Check if the request was successful
            if (resp.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pantry/Remove/5
        [Authorize]
        public ActionResult Remove(int id)
        {
            return View();
        }

        // POST: Pantry/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            GetApplicationCookie();
            // Define url path to the API Endpoint for deleting a pantry
            string url = $"PantryData/DeletePantry/{id}";

            // Format post data to be HTTP readable
            HttpContent content = new StringContent("");
            // Send post request, including empty content and storing response
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            // Check if request was successful
            if(resp.IsSuccessStatusCode)
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
