using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GS_Microsservicos.Models
{
    public class Consumptiondomain
    {
    
        public String Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Consumption { get; set; }
    }
}
