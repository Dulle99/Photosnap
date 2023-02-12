using MongoDB.Bson;
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
           if (await GetDocumentByFieldValue(userCollection,"Username", username) != null)
                return true;

            return false;
        }

        public async static Task<TDocument> GetDocumentByFieldValue<TDocument, TValue>(IMongoCollection<TDocument> collection,string fieldName, TValue fieldValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(fieldName, fieldValue);
            var cursor = await collection.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public static async Task UpdateFieldInCollecton<TDocument,TFilterFieldVal,TUpdateFieldValue>(IMongoCollection<TDocument> collection, string filterFieldName, TFilterFieldVal filterFieldValue,
                                                                 string updateFieldName, TUpdateFieldValue updateFieldValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterFieldName, filterFieldValue);
            var update = Builders<TDocument>.Update.Set(updateFieldName, updateFieldValue);
            await collection.UpdateOneAsync(filter, update);
        }

        public static async Task<bool> RemoveDocument<TDocument>(IMongoCollection<TDocument> collection, string filterFieldName, ObjectId documentId)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterFieldName, documentId);
            var deleteResult = await collection.DeleteOneAsync(filter);
            if (deleteResult.IsAcknowledged)
                return true;
            return false;
        }
    }
}
