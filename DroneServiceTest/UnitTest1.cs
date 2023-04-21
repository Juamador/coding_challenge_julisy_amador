using DroneDeliveryService.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace DroneServiceTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CallValidatorFileWithInvalidData()
        {
            //Arrage
            var Drones = "";
            var PackageLocatoin = new List<string>(0);
            ILogService LogService = new LogService();

            //Act
            IDroneFileValidatorService Service = new DroneFileValidatorService(LogService);
            var ServiceResponse = Service.ValidateFile(Drones, PackageLocatoin);

            //Assert
            Assert.False(ServiceResponse.Succeded);
        }

        [Test]
        public void CallValidatorFileWithValidData()
        {
            //Arrage
            var Drones = "[DroneA], [200], [DroneB], [250], [DroneC], [100]";
            var PackageLocatoin = new List<string>(0);
            PackageLocatoin.Add(new string("[LocationA], [200]"));
            PackageLocatoin.Add(new string("[LocationB], [150]"));
            PackageLocatoin.Add(new string("[LocationC], [50]"));
            PackageLocatoin.Add(new string("[LocationD], [150]"));
            PackageLocatoin.Add(new string("[LocationE], [100]"));
            PackageLocatoin.Add(new string("[LocationF], [200]"));
            PackageLocatoin.Add(new string("[LocationG], [50]"));
            PackageLocatoin.Add(new string("[LocationH], [80]"));
            PackageLocatoin.Add(new string("[LocationI], [70]"));
            PackageLocatoin.Add(new string("[LocationJ], [50]"));
            PackageLocatoin.Add(new string("[LocationK], [30]"));
            PackageLocatoin.Add(new string("[LocationL], [20]"));
            PackageLocatoin.Add(new string("[LocationM], [50]"));
            PackageLocatoin.Add(new string("[LocationN], [30]"));
            PackageLocatoin.Add(new string("[LocationO], [20]"));
            PackageLocatoin.Add(new string("[LocationP], [90]"));

            ILogService LogService = new LogService();

            //Act
            IDroneFileValidatorService Service = new DroneFileValidatorService(LogService);
            var ServiceResponse = Service.ValidateFile(Drones, PackageLocatoin);

            //Assert
            Assert.True(ServiceResponse.Succeded);
        }

        [Test]
        public void CallOutputFileGenerator()
        {
            //Arrage
            var Drones = "[DroneA], [200], [DroneB], [250], [DroneC], [100]";
            var PackageLocatoin = new List<string>(0);
            PackageLocatoin.Add(new string("[LocationA], [200]"));
            PackageLocatoin.Add(new string("[LocationB], [150]"));
            PackageLocatoin.Add(new string("[LocationC], [50]"));
            PackageLocatoin.Add(new string("[LocationD], [150]"));
            PackageLocatoin.Add(new string("[LocationE], [100]"));
            PackageLocatoin.Add(new string("[LocationF], [200]"));
            PackageLocatoin.Add(new string("[LocationG], [50]"));
            PackageLocatoin.Add(new string("[LocationH], [80]"));
            PackageLocatoin.Add(new string("[LocationI], [70]"));
            PackageLocatoin.Add(new string("[LocationJ], [50]"));
            PackageLocatoin.Add(new string("[LocationK], [30]"));
            PackageLocatoin.Add(new string("[LocationL], [20]"));
            PackageLocatoin.Add(new string("[LocationM], [50]"));
            PackageLocatoin.Add(new string("[LocationN], [30]"));
            PackageLocatoin.Add(new string("[LocationO], [20]"));
            PackageLocatoin.Add(new string("[LocationP], [90]"));

            ILogService LogService = new LogService();

            //Act
            IDroneFileValidatorService Service = new DroneFileValidatorService(LogService);
            IDronesDeliveryService DroneDeliveryService = new DronesDeliveryServices(LogService);
            var ServiceResponse = Service.ValidateFile(Drones, PackageLocatoin);
            var DistributionResponse = DroneDeliveryService.SetPackageDistributionByDrone(ServiceResponse.Record.Drones, ServiceResponse.Record.PackagesLocation);
            
            //Assert
            Assert.AreNotEqual("", DistributionResponse.OutputFilePath);
        }

    }
}