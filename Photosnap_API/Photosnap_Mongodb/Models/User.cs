using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Models
{
    public class User
    {
        #region Filed(s)

        [BsonId]
        public ObjectId UserId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhotoFilePath { get; set; }

        public int NumberOfPublishedPictures { get; set; }

        public int NumberOfFollowers { get; set; }

        public byte[] Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        public List<MongoDBRef> UserPhotos { get; set; }

        public List<PhotoCategory> PhotoCategoriesOfInterest { get; set; }

        #endregion Filed(s)

        #region Constructor

        public User() 
        {
            PhotoCategoriesOfInterest= new List<PhotoCategory>();
            PasswordSalt = new Guid().ToByteArray();
            NumberOfFollowers= 0;
            NumberOfPublishedPictures= 0;
        }

        #endregion Constructor
    }
}
