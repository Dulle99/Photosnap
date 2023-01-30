using MongoDB.Driver;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.ServiceHelpMethods
{
    public static class HelpMethods
    {

        public async static Task<bool> UsernameIsTaken(IMongoCollection<User> userCollection, string username)
        { 
           if (await GetUserByUsername(userCollection, username) != null)
                return true;

            return false;
        }

        public async static Task<T> GetDocumentByFieldValue<T>(IMongoCollection<T> collection,string fieldName, string fieldValue)
        {
            //TODO: Test 
            var filter = Builders<T>.Filter.Eq(fieldName, fieldValue);
            var cursor = await collection.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public async static Task<User> GetUserByUsername(IMongoCollection<User> userCollection, string username)
        {
            var filter = Builders<User>.Filter.Eq("Username", username);
            var cursor = await userCollection.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public async static Task<PhotoCategory> GetCategoryByName(IMongoCollection<PhotoCategory> photoCategoryCollection, string categoryName)
        {
            var filter = Builders<PhotoCategory>.Filter.Eq("CategoryName", categoryName);
            var cursor = await photoCategoryCollection.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

    }
}
