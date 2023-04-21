using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryService.Services
{
    public interface ILogService
    {
        void Log(string Message);
    }
    public class LogService : ILogService
    {
        public void Log(string Message)
        {
            var FolderName = Path.Combine("Resources", "Logs");
            var PathToSaveDocument = Path.Combine(Directory.GetCurrentDirectory(), FolderName);
            var FileName = new Guid() + ".txt";
            var FullPath = Path.Combine(PathToSaveDocument, FileName);
            File.WriteAllText(FullPath, Message);

        }
    }
}
