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

        public int NumberOfFollowers { get; set; }

        public byte[] Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        public List<ObjectId> FollowingUsers { get; set; }

        public List<ObjectId> FollowersOfUser { get; set; }

        public List<ObjectId> UserLikes { get; set; }

        public List<MongoDBRef> UserPhotos { get; set; }

        public List<PhotoCategory> PhotoCategoriesOfInterest { get; set; }

        #endregion Filed(s)

        #region Constructor

        public User() 
        {
            FollowingUsers = new List<ObjectId>();
            FollowersOfUser = new List<ObjectId>(); 
            PhotoCategoriesOfInterest= new List<PhotoCategory>();
            UserPhotos = new List<MongoDBRef>(); 
            PasswordSalt = new Guid().ToByteArray();
            NumberOfFollowers= 0;
        }

        #endregion Constructor
    }
}
