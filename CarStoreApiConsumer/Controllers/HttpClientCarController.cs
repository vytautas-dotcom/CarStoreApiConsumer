using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            return Redirect("/HttpClientStore/StoreList");
        }
    }
}
