using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryService.Helpers
{
    public static class Utilities
    {
        
        public static string GetFullExceptionLog(Exception ExceptionData)
        {
            var output = ExceptionData.Message;

            if (ExceptionData.InnerException != null)
                output += Environment.NewLine + " InnerException: " + ExceptionData.InnerException.Message;

            return output;
        }
    }
}
