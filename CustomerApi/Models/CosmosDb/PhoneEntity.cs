namespace CustomerApi.Models.CosmosDb
{
    public class PhoneEntity
    {
        //[BsonElement("Type")]
        public PhoneType Type { get; set; }
        //[BsonElement("AreaCode")]
        public int AreaCode { get; set; }
        //[BsonElement("Number")]
        public int Number { get; set; }
    }
}