using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiGateway.Client.Models
{
    public class WebSocketRequest
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
