using DroneDeliveryService.DTO;
using DroneDeliveryService.Helpers;
using DroneDeliveryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace DroneDeliveryService.Services
{
    public interface IDronesDeliveryService
    {
        Response<DroneDistributionResult> SetPackageDistributionByDrone(List<Drone> Drones, List<PackageDeliveryAddress> PackageDeliveryAddresses);
    }
    public class DronesDeliveryServices : IDronesDeliveryService 
    {
        private readonly ILogService _LogService;
        public DronesDeliveryServices(ILogService LogService)
        {
            _LogService = LogService;
        }
        public Response<DroneDistributionResult> SetPackageDistributionByDrone(List<Drone> Drones, List<PackageDeliveryAddress> PackageDeliveryAddresses)
        {
            var response = new Response<DroneDistributionResult>();
            var trip = 1;
            try
            {
                response.EntityList = new List<DroneDistributionResult>(0);
                var DistinctDeliveryLocation = PackageDeliveryAddresses.Select(location => location.Location).Distinct();
                var SortedDronesByDescending = Drones.OrderByDescending(drone => drone.Weight);
                var SortedLocationsByDescending = PackageDeliveryAddresses.OrderByDescending(drone => drone.Weight);
                var TotalWeightToDelivery = PackageDeliveryAddresses.Sum(location => location.Weight);
                var DroneDinstributions = new List<DroneDistributionResult>(0);
                while (TotalWeightToDelivery > response.EntityList.Sum(location => location.Weight))
                {
                    foreach (var drone in SortedDronesByDescending)
                    {
                        var DroneDistribution = new DroneDistributionResult { DroneName = drone.DroneName, Trip = trip, Weight = drone.Weight };
                        foreach (var PackageLocation in DistinctDeliveryLocation)
                        {
                            var PackagesInTheSameLocation = SortedLocationsByDescending.Where(location => location.Location == PackageLocation && !location.IsLoad).ToList().OrderByDescending(location => location.Weight).ToList();
                            foreach (var locaton in PackagesInTheSameLocation)
                            {
                                if (!DroneDistribution.IsFull)
                                {
                                    var MissingQuantityToFill = DroneDistribution.Weight - DroneDistribution.TotalWeight;
                                    if (locaton.Weight <= MissingQuantityToFill)
                                    {
                                        DroneDistribution.Locations.Add(new LocationLoad { Location = locaton.Location, Weight = locaton.Weight, IsLoaded = true });
                                        SortedLocationsByDescending.FirstOrDefault(x => x.Location == locaton.Location).IsLoad = true;
                                    }
                                }
                            }
                        }
                        DroneDinstributions.Add(DroneDistribution);
                    }
                    response.EntityList = DroneDinstributions;
                    trip++;
                }              
            }
            catch (System.Exception ex)
            {
                var FullMessage = Utilities.GetFullExceptionLog(ex);
                _LogService.Log(FullMessage);
            }
            return response;
        }
    }
}
