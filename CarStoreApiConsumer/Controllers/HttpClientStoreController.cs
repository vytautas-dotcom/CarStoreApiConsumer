using CarStoreApiConsumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarStoreApiConsumer.Controllers
{
    public class HttpClientStoreController : Controller
    {
        public async Task<IActionResult> StoreList(Guid storeId = default)
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
            var id = storeId != Guid.Empty ? storeId : (from store in storeList select store.Id).First();
            ViewBag.storeId = id;
            return View(storeList);
        }


        public IActionResult AddStore()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStore(Store store, Guid storeID)
        {
            Store receivedStore = new Store();
            if (store.Id == Guid.Empty && storeID == Guid.Empty)
            {
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
            }
            else
            {
                store.Id = storeID;
                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(store),
                                                                    encoding: Encoding.UTF8,
                                                                    mediaType: "application/json");
                        using (var response = await httpClient.PutAsync("https://localhost:44372/stores/", stringContent))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            receivedStore = JsonConvert.DeserializeObject<Store>(apiResponse);
                        }
                }
            }
            ViewBag.Id = receivedStore.Id != Guid.Empty ? receivedStore.Id : storeID;
            return View(receivedStore);
        }

    }
}
