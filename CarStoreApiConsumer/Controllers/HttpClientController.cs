using CarStoreApiConsumer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarStoreApiConsumer.Controllers
{
    public class HttpClientController : Controller
    {
        public async Task<IActionResult> StoreList()
        {
            List<Store> storeList = new List<Store>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44372/stores/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    storeList = JsonConvert.DeserializeObject<List<Store>>(apiResponse);
                }
            }
            return View(storeList);
        }

        public IActionResult AddStore()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStore(Store store)
        {
            Store receivedStore = new Store();
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(store),
                                                                encoding: Encoding.UTF8,
                                                                mediaType: "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44372/stores/", stringContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedStore = JsonConvert.DeserializeObject<Store>(apiResponse);
                    ViewBag.StatusCode = (int)response.StatusCode;
                }
            }
            return View(receivedStore);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStore(Guid storeId, Guid carId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44372/stores/"+storeId+"/cars/"+carId))
                {

                }
            }
            return RedirectToAction(nameof(StoreList));
        }
    }
}
