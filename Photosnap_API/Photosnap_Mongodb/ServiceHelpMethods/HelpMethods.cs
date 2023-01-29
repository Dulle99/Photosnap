using MongoDB.Driver;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.ServiceHelpMethods
{
    public static class HelpMethods
    {

        public async static Task<bool> UsernameIsTaken(IMongoDatabase _db, string username)
        {
            var userCollection = _db.GetCollection<User>(PhotosnapCollection.User);

            var filter = Builders<User>.Filter.Eq("Username", username);
            var res = await userCollection.FindAsync(filter);
            
            if (await res.FirstOrDefaultAsync() != null)
                return true;

            return false;
        }

    }
}
