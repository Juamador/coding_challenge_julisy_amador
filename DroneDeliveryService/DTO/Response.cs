using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DroneDeliveryService.DTO
{
    public class Response
    {
        public bool Succeded => !Errors.Any();
        public string OutputFilePath { get; set; }
        public List<string> Errors { get; set; } = new List<string>(0);
    }

    public class Response<T> : Response where T : class
    {
        public List<T> EntityList { get; set; }
        public T Record { get; set; }
    }
}
