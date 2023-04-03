using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.UserDTO
{
    public class UserProfilePreviewDTO
    {
        #region Filed(s)

        public string Username { get; set; }

        public byte[] ProfilePhoto { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public int NumberOfFollowers { get; set; }

        public int numberOfFollowings { get; set; }

        public int NumberOfCategoriesOfInterst { get; set; }

        #endregion Filed(s)

        #region Constructor

        public UserProfilePreviewDTO() { }

        #endregion Constructor
    }
}
