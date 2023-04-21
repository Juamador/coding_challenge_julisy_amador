using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryService.Models
{
    public class PackageDeliveryAddress
    {
        public string Location { get; set; }
        public int Weight { get; set; }
        public bool IsLoad { get; set; } = false;
    }
}
