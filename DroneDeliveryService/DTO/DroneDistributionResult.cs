using DroneDeliveryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace DroneDeliveryService.DTO
{
    public class DroneDistributionResult: Drone
    {
        public DroneDistributionResult()
        {
            this.Locations = new List<LocationLoad>(0);
        }

        public int Trip { get; set; }
        public bool IsFull => TotalWeight == Weight;
        public int TotalWeight => Locations.Sum(x => x.Weight);
        public List<LocationLoad> Locations { get; set; }
    }

    public class LocationLoad : PackageDeliveryAddress
    {
        public bool IsLoaded { get; set; } = false;
    }
}
