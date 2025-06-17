
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GraziosoBlazorApp.Models
{
    // needed to deserialize data for MongoDB
    [BsonDiscriminator("RescueAnimal", Required = true)]
    [BsonKnownTypes(typeof(Dog),  typeof(Monkey))]

    // Defines Models for Dogs and Monkeys
    public abstract class RescueAnimal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public double Weight {  get; set; }
        public DateTime AcquisitionDate { get; set; } = DateTime.Today;
        public string? AcquisitionCountry { get; set; }
        public string? TrainingStatus { get; set; }
        public bool Reserved {  get; set; }
        public string? InServiceCountry { get; set; }
      
    }

    [BsonDiscriminator("Dog")]
    public class Dog : RescueAnimal
    {
        public string? Breed { get; set; }
    }

    [BsonDiscriminator("Monkey")]
    public class Monkey : RescueAnimal
    {
        public string? Species { get; set; }
        public double TailLength { get; set; }
        public double Height { get; set; }
        public double BodyLength {  get; set; }
    }

}
