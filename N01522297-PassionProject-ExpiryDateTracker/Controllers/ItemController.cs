using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using N01522297_PassionProject_ExpiryDateTracker.Models;
using N01522297_PassionProject_ExpiryDateTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace N01522297_PassionProject_ExpiryDateTracker.Controllers
{
    // Item Controller that communicates with the ItemDataController to then render results to the views
    public class ItemController : Controller
    {
        // Create a static instance of the http client connection to allow reuse
        private static readonly HttpClient client;
        // Create new javascript serializer
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static ItemController() {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            // Define the base url string that all Item requests share
            client.BaseAddress = new Uri("https://localhost:44332/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;
            if (token != null) client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Item/List
        [Authorize]
        public ActionResult List(string ItemSearch = null)
        {
            GetApplicationCookie();
            string url = "ItemData/ListItems";

            if (ItemSearch != null)
            {
                url += "?ItemSearch=" + ItemSearch;
            }

            HttpResponseMessage resp = client.GetAsync(url).Result;

            // Parse response message into proper format
            IEnumerable<ItemDto> items = resp.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;

            return View(items);
        }

        // GET: Item/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            string url = $"ItemData/FindItem/{id}";
            HttpResponseMessage resp = client.GetAsync(url).Result;

            // Parse response message into proper format
            ItemDto item = resp.Content.ReadAsAsync<ItemDto>().Result;

            return View(item);
        }
        [Authorize]
        public ActionResult Error()
        {
            return View();
        }

        // GET: Item/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            // Retrieve the Pantries to allow the user to select from one
            string url = "PantryData/ListPantries";

            // Send request to API
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Retrieve the list of pantries
            IEnumerable<PantryDto> pantryOptions = resp.Content.ReadAsAsync<IEnumerable<PantryDto>>().Result;

            return View(pantryOptions);
        }

        // POST: Item/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Item newItem)
        {
            GetApplicationCookie();
            string url = "ItemData/AddItem";
            // Convert passed in Item type into a JSON string
            string payload = serializer.Serialize(newItem);

            // Convert the payload json data into HttpContent that is read by the post request
            HttpContent content = new StringContent(payload);
            // Set the content type header to be application/json
            content.Headers.ContentType.MediaType = "application/json";
            // Send the json payload to the respective url
            HttpResponseMessage resp = client.PostAsync(url, content).Result;

            if (resp.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Item/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            UpdateItem itemVM = new UpdateItem();

            // Create path to finding animal
            string url = $"ItemData/FindItem/{id}";
            // Set asynchronous request
            HttpResponseMessage resp = client.GetAsync(url).Result;
            // Store resulting ItemDto
            ItemDto itemDto = resp.Content.ReadAsAsync<ItemDto>().Result;
            // Store the item dto in the created ViewModel
            itemVM.SelectedItem = itemDto;

            // Retrieve the Pantries to allow the user to select from one
            url = "PantryData/ListPantries";
            // Send request to API
            resp = client.GetAsync(url).Result;
            // Retrieve the list of pantries
            IEnumerable<PantryDto> pantryOptions = resp.Content.ReadAsAsync<IEnumerable<PantryDto>>().Result;
            // Store the retrieved pantries in the ViewModel
            itemVM.PantryOptions = pantryOptions;

            return View(itemVM);
        }

        // POST: Item/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Item updateItem)
        {
            GetApplicationCookie();
            // Define api url path to the update api
            string url = $"ItemData/UpdateItem/{id}";

            // Convert the updated item into a json string
            string payload = serializer.Serialize(updateItem);
            // Format the payload into a request readable object
            HttpContent content = new StringContent(payload);
            // Set header type to reflect the json data
            content.Headers.ContentType.MediaType = "application/json";
            // Make post request, storing returned response
            HttpResponseMessage resp = client.PostAsync(url,content).Result;

            if (resp.IsSuccessStatusCode)
            {
                // TODO: Add update logic here

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Item/Remove/5
        [Authorize]
        public ActionResult Remove(int id)
        {
            return View();
        }

        // POST: Item/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            GetApplicationCookie();
            // Define url path to the delete iten api
            string url = $"ItemData/DeleteItem/{id}";
            // Convert empty body data to HttpContent
            HttpContent content = new StringContent("");
            // Make http request to desire url, sending empty body data
            HttpResponseMessage resp = client.PostAsync(url,content).Result;

            // Check if result was successful
            if (resp.IsSuccessStatusCode) {
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
