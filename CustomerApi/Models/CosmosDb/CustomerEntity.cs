using CustomerApi.Models.Repositories;
using System.Collections.Generic;

namespace CustomerApi.Models.CosmosDb
{
    public class CustomerEntity:CustomerObject
    {
        //[BsonElement("Id")]
        public long Id { get; set; }
        //[BsonElement("Email")]
        public string Email { get; set; }
        //[BsonElement("Name")]
        public string Name { get; set; }
        //[BsonElement("Age")]
        public int Age { get; set; }
        //[BsonElement("Phones")]
        public List<PhoneEntity> Phones { get; set; }
    }
}
