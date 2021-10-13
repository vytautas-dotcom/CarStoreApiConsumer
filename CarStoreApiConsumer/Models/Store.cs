using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStoreApiConsumer.Models
{
    public class Store
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public List<Car> CarList { get; set; }
    }
}
