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


        public async Task<IActionResult> AddStore(string readOnly = null, Guid storeId = default)
        {
            Store receivedStore = new Store();
            if (storeId != Guid.Empty)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/stores/"+ storeId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedStore = JsonConvert.DeserializeObject<Store>(apiResponse);
                        ViewBag.StatusCode = (int)response.StatusCode;
                    }
                }
                ViewBag.Id = storeId;
                ViewBag.ReadOnly = readOnly;
                return View(receivedStore);
                
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStore(Store store = null, Guid storeId = default)
        {
            Store receivedStore = new Store();
            if (store.Id == Guid.Empty && storeId == Guid.Empty)
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
                store.Id = storeId;
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
            ViewBag.Id = receivedStore.Id != Guid.Empty ? receivedStore.Id : storeId;
            return RedirectToAction("StoreList", new { storeId = storeId});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStore(Guid storeId)
        {
            bool isSuccess;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44372/stores/" + storeId))
                {
                    isSuccess = response.IsSuccessStatusCode;
                }
            }
            if (isSuccess)
            {
                return RedirectToAction("StoreList");
            }
            else
            {
                return BadRequest("Something went wrong!");
            }
        }

    }
}
