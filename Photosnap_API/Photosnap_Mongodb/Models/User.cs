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

        public string Username { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public string ProfilePictureFilePath { get; set; }

        public int NumberOfPublishedPictures { get; set; }

        public byte[] Password { get; set; }

        public byte[] PasswordSalt { get; set; }

        public List<PhotoCategory> PhotoCategoriesOfInterest { get; set; }

        //Razmisliti da li i ovo ubaciti
        //public List<MongoDBRef> FollowingUsers { get; set; } 

        #endregion Filed(s)

        #region Constructor

        public User() 
        {
            PhotoCategoriesOfInterest= new List<PhotoCategory>();
        }

        #endregion Constructor
    }
}
