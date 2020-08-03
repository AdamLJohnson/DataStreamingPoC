using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Client.Models
{
    public class KinesisUpdate
    {
        public Data data { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Data
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class Metadata
    {
        public DateTime timestamp { get; set; }
        public string recordtype { get; set; }
        public string operation { get; set; }
        public string partitionkeytype { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int transactionid { get; set; }
    }
}
