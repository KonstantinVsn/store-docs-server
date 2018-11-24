using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreDocApi.Models
{
    public class FileBlock
    {
        [BsonId]
        public string Id { get; set; }
        public string Hash { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileBody { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
