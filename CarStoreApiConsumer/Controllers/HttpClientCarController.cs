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
    public class HttpClientCarController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> DeleteCar(Guid storeId, Guid carId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44372/stores/" + storeId + "/cars/" + carId))
                {

                }
            }
            return RedirectToAction("StoreList", "HttpClientStore", new { storeId = storeId});
        }
        public async Task<IActionResult> UpdateCar(Guid storeId, Guid carId)
        {
            List<Car> carList = new List<Car>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44372/stores/" + storeId + "/cars/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    carList = JsonConvert.DeserializeObject<List<Car>>(apiResponse);
                }
            }
            var car = (from c in carList where c.Id == carId select c).First();
            ViewBag.StoreId = storeId;
            return View(car);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCar(Guid storeId, Guid carId, Car car)
        {
            bool isSuccess;
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(car),
                                                                Encoding.UTF8,
                                                                "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:44372/stores/" + storeId + "/cars/", 
                                                                stringContent))
                {
                    isSuccess = response.IsSuccessStatusCode;
                }
            }
            if (isSuccess)
            {
                return RedirectToAction("StoreList", "HttpClientStore", new { storeId = storeId });
            }
            else
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
