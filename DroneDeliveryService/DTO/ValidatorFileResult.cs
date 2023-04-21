using DroneDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryService.DTO
{
    public class ValidatorFileResult
    {
        public ValidatorFileResult()
        {
            this.Drones = new List<Drone>(0);
            this.PackagesLocation = new List<PackageDeliveryAddress>(0);
        }
        public List<Drone> Drones { get; set; }
        public List<PackageDeliveryAddress> PackagesLocation{ get; set; }
    }
}
