using DroneDeliveryService.DTO;
using DroneDeliveryService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DroneDeliveryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroneSquadController : ControllerBase
    {
        private readonly IDroneFileValidatorService _IDroneFileValidatorService;
        private readonly IDronesDeliveryService _DronesDeliveryService;

        public DroneSquadController(IDroneFileValidatorService DroneFileValidatorService, IDronesDeliveryService DronesDeliveryService)
        {
            _IDroneFileValidatorService = DroneFileValidatorService;
            _DronesDeliveryService = DronesDeliveryService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult SetDronePayload()
        {   
            var response = new Response();
            string DroneSquad = string.Empty;
            var DeliveryLocation = new List<string>(0);
            try
            {
                var Document = Request.Form.Files[0];
                if (Document.Length == 0)
                {
                    response.Errors.Add("File not found");
                    return Ok(response);
                }
                
                var FolderName = Path.Combine("Resources", "DroneSquad");
                var PathToSaveDocument = Path.Combine(Directory.GetCurrentDirectory(), FolderName);
                var FileName = ContentDispositionHeaderValue.Parse(Document.ContentDisposition).FileName.Trim('"');
                var FullPathTmp = Path.Combine(PathToSaveDocument, FileName);
                var FullPath = Path.Combine(PathToSaveDocument, FileName);
                var FilePath = Path.Combine(FolderName, FileName);

                using (var stream = new FileStream(FullPath, FileMode.Create))
                {
                    Document.CopyTo(stream);
                }

                string[] lines = System.IO.File.ReadAllLines(FullPath);
                var LineNumber = 0;
                foreach (string line in lines)
                {
                    if (LineNumber == 0)
                        DroneSquad = line;
                    else
                        DeliveryLocation.Add(line);

                    LineNumber++;
                }
                var FileValidationResult = _IDroneFileValidatorService.ValidateFile(DroneSquad, DeliveryLocation);
               var DistributionResponse = _DronesDeliveryService.SetPackageDistributionByDrone(FileValidationResult.Record.Drones, FileValidationResult.Record.PackagesLocation);


                if (System.IO.File.Exists(FullPath)) System.IO.File.Delete(FullPath);
                var OutputFile= Path.Combine(FolderName, "Output.txt");
                string FileContent = null;
                var DistinctDrones =DistributionResponse.EntityList.Select(x => x.DroneName).Distinct().ToArray();
                Array.Sort(DistinctDrones);
                foreach (var FilteredDrone in DistinctDrones)
                {
                    if (!string.IsNullOrEmpty(FileContent))
                    {
                        FileContent += Environment.NewLine + FilteredDrone;
                    }
                    else
                    {
                        FileContent = FilteredDrone;
                    }

                    var Drones = DistributionResponse.EntityList.Where(drone => drone.DroneName == FilteredDrone);
                    foreach (var drone in Drones)
                    {
                        if (drone.TotalWeight > 0)
                        {
                            FileContent += Environment.NewLine + "Trip #" + drone.Trip.ToString() + Environment.NewLine;
                            var index = 0;
                            foreach (var PackageLocation in drone.Locations)
                            {
                                if (index < drone.Locations.Count() - 1)
                                    FileContent += PackageLocation.Location + ",";
                                else
                                    FileContent += PackageLocation.Location + Environment.NewLine;
                            }
                        }
                    }
                }
                
                
                System.IO.File.WriteAllText(OutputFile, FileContent);
                response.OutputFilePath = OutputFile;

            }
            catch (Exception ex)
            {
                response.Errors.Add("An error has occurred saving the document. error detail: \n" + ex.Message);
            }
            return Ok(response);
        }
    }
}
