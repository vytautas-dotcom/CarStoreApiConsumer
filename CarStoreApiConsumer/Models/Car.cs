using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStoreApiConsumer.Models
{
    public class Car
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DateRelease { get; set; }
        public int Price { get; set; }
        public string Remark { get; set; }
        public bool IsInStore { get; set; }
    }
}
