using DroneDeliveryService.DTO;
using DroneDeliveryService.Helpers;
using DroneDeliveryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryService.Services
{
    public interface IDroneFileValidatorService
    {
        Response<ValidatorFileResult> ValidateFile(string DroneSquad, List<string> DeliveryLocations);
    }
    public class DroneFileValidatorService : IDroneFileValidatorService
    {
        private readonly ILogService _LogService;
        public DroneFileValidatorService(ILogService LogService)
        {
            _LogService = LogService;
        }
        public Response<ValidatorFileResult> ValidateFile(string DroneSquad, List<string> DeliveryLocations)
        {
            var response = new Response<ValidatorFileResult>();
            try
            {
                var Validation = ValidateDronesAndPackagesLocations(DroneSquad, DeliveryLocations);
                if (!Validation.Succeded)
                {
                    response.Errors = Validation.Errors;
                    return response;
                }
                response.Record = new ValidatorFileResult();
                response.Record.Drones = ExtractDronesFromString(DroneSquad);
                response.Record.PackagesLocation = ExtractPackagesLocatoinFromString(DeliveryLocations);

            }
            catch (Exception ex)
            {
                response.Errors.Add("An error occurred validating the file.");
                var FullMessage = Utilities.GetFullExceptionLog(ex);
                _LogService.Log(FullMessage);
            }
            return response;
        }

        private List<Drone> ExtractDronesFromString(string DroneSquad)
        {
            var result = new List<Drone>(0);
            var Drones = DroneSquad.Split(',');

            List<int> DronesWeight = new List<int>(0);
            List<string> DronesName = new List<string>(0);
            for (var i = 0; i <= Drones.Length - 1; i++)
            {
                Drones[i] = Drones[i].Trim();
                // validating if the value is a number to take it as weight information
                var UnformatedValue = Drones[i].ToString().Replace("[", "").Replace("]", "").Replace(" ", "");
                if (char.IsNumber(UnformatedValue, 0))
                    DronesWeight.Add(Convert.ToInt32(UnformatedValue));
                else
                    DronesName.Add(Drones[i]);
            }

            for (var i = 0; i <= DronesName.Count() - 1; i++)
            {
                result.Add(new Drone { DroneName = DronesName[i], Weight = DronesWeight[i] });
            }
            return result;
        }

        private List<PackageDeliveryAddress> ExtractPackagesLocatoinFromString(List<string> DeliveryLocations)
        {
            var result = new List<PackageDeliveryAddress>(0);

            foreach (var location in DeliveryLocations)
            {
                var Packages = location.Split(',');

                List<int> LocationWeight = new List<int>(0);
                List<string> LocationName = new List<string>(0);
                for (var i = 0; i <= Packages.Length - 1; i++)
                {
                    // validating if the value is a number to take it as weight information
                    var UnformatedValue = Packages[i].ToString().Replace("[", "").Replace("]", "").Replace(" ", "");
                    if (char.IsNumber(UnformatedValue, 0))
                        LocationWeight.Add(Convert.ToInt32(UnformatedValue));
                    else
                        LocationName.Add(Packages[i]);
                }

                for (var i = 0; i <= LocationName.Count() - 1; i++)
                {
                    result.Add(new PackageDeliveryAddress { Location = LocationName[i], Weight = LocationWeight[i], IsLoad=false });
                }
            }


            return result;
        }

        private Response ValidateDronesAndPackagesLocations(string DroneSquad, List<string> DeliveryLocations)
        {
            var response = new Response();
            if (string.IsNullOrWhiteSpace(DroneSquad))
            {
                response.Errors.Add("Sorry, this file is invalid because it does not contain drones.");
            }

            var Drones = DroneSquad.Split(',');
            if (Drones.Length == 0)
            {
                response.Errors.Add("The drone list is incorrect");
            }

            if (DeliveryLocations.Count() <= 0)
            {
                response.Errors.Add("Sorry, this file is invalid because it does not contain any packages to deliver.");
            }
            return response;
        }
    }
}

