using MongoDB.Driver;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.Service;

namespace Photosnap_API.MongoIndexing
{
    public static class MongoDbIndexing
    {
        public static async void InitializeIndexes(IMongoDatabase mongoDatabase)
        {

            var userCollection = mongoDatabase.GetCollection<User>(PhotosnapCollections.User);
            var categoryCollection = mongoDatabase.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);

            var userIndex_keyDefinition = Builders<User>.IndexKeys.Text("Username");
            await userCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(userIndex_keyDefinition));

            userIndex_keyDefinition = Builders<User>.IndexKeys.Descending("UserLikes");
            await userCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(userIndex_keyDefinition));

            userIndex_keyDefinition = Builders<User>.IndexKeys.Descending("PhotoLikes");
            await userCollection.Indexes.CreateOneAsync(new CreateIndexModel<User>(userIndex_keyDefinition));

            var categoryIndex_keyDefinition = Builders<PhotoCategory>.IndexKeys.Text("CategoryName");
            await categoryCollection.Indexes.CreateOneAsync(new CreateIndexModel<PhotoCategory>(categoryIndex_keyDefinition));

        }

        
    }
}
