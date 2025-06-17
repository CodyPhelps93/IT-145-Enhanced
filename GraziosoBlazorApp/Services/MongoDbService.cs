using MongoDB.Driver;
using GraziosoBlazorApp.Models;

namespace GraziosoBlazorApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<RescueAnimal> _animalsCollection;

        public MongoDbService()
        { 
            var client = new MongoClient("mongodb://localhost:27017/"); // 
            var database = client.GetDatabase("Grazioso");
            _animalsCollection = database.GetCollection<RescueAnimal>("Animals");

            InitializeData();
        }

        // Start DB with random data
        private void InitializeData()
        {
            var count = _animalsCollection.CountDocuments(_ => true);
            if (count == 0)
            {
                var dog1 = new Dog { Name = "Spot", Breed = "German Shepherd", Gender = "male", Age = 1, Weight = 25.6, AcquisitionDate = new DateTime(2015,05,12), 
                    AcquisitionCountry = "United States", TrainingStatus = "intake", Reserved = false, InServiceCountry = "United States", Type = "Dog" };

                var dog2 = new Dog { Name = "Rex", Breed = "Great Dane", Gender = "male", Age = 3, Weight = 35.2, AcquisitionDate = new DateTime(2020, 02,03), 
                    AcquisitionCountry = "United States", TrainingStatus = "Phase I", Reserved = false, InServiceCountry = "United States", Type = "Dog" };

                var dog3 = new Dog { Name = "Bella", Breed = "Chihuahua", Gender = "female", Age = 4, Weight = 25.6, AcquisitionDate = new DateTime(2012, 3 ,15), 
                    AcquisitionCountry = "Canada", TrainingStatus = "in service", Reserved = true, InServiceCountry = "Canada", Type = "Dog" };

                var monkey1 = new Monkey { Name = "Sharp", Gender = "male", Age = 17, Weight = 9, AcquisitionDate = new DateTime(2015, 03, 15), 
                    AcquisitionCountry = "United States", TrainingStatus = "in service", Reserved = false, 
                    InServiceCountry = "United States", Species = "Spider Monkey", TailLength = 15.8, Height = 16.7, BodyLength = 12.22, Type = "Monkey" };

                _animalsCollection.InsertMany(new RescueAnimal[] {dog1, dog2, dog3, monkey1});
            }

        }

        //Animal Search and CRUD operations
        public async Task<List<RescueAnimal>> GetAllAnimalsAsync()
        {
            try
            {
                return await _animalsCollection.Find(_ => true).ToListAsync();
            } catch (Exception e)
            { Console.WriteLine(e);
                return new List<RescueAnimal>();  }

        }

        public async Task<List<RescueAnimal>> GetAnimalByTypeAsync(string type) // type can be dog or monkey
        {
           return await _animalsCollection.Find(animal => animal.Type == type).ToListAsync();
        }

        public async Task<List<RescueAnimal>> GetAvailableAnimalAsync()
        {
            return await _animalsCollection.Find(animal => animal.TrainingStatus == "in service" && !animal.Reserved).ToListAsync();
        }

        public async Task<bool> InsertAnimalAsync(RescueAnimal rescueAnimal)
        {
            var exists = await _animalsCollection.Find(animal => animal.Name == rescueAnimal.Name && animal.Type == rescueAnimal.Type).FirstOrDefaultAsync();
            if (exists != null)
                return false;

            await _animalsCollection.InsertOneAsync(rescueAnimal);
            return true;
        }

        public async Task UpdateAnimalAsync(string id, RescueAnimal rescueAnimal)
        {
            await _animalsCollection.ReplaceOneAsync(animal => animal.Id == id, rescueAnimal);
        }

        public async Task DeleteAnimalAsync(string id)
        {
            await _animalsCollection.DeleteOneAsync(animal => animal.Id == id);
        }

        // Function used to filter and reserve animal
        public async Task<bool> ReserveAnimalAsync(string type, string country, string name)
        {
            var filter = Builders<RescueAnimal>.Filter.Eq(animal => animal.Type, type) &
                Builders<RescueAnimal>.Filter.Eq(animal => animal.AcquisitionCountry, country) &
                Builders<RescueAnimal>.Filter.Eq(animal => animal.Name, name) &
                Builders<RescueAnimal>.Filter.Eq(animal => animal.Reserved, false);
            var update = Builders<RescueAnimal>.Update.Set(animal => animal.Reserved, true);
            var result = await _animalsCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }


    }


}
