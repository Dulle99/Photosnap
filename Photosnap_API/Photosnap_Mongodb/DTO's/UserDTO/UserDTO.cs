using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Photosnap_Mongodb.DTO_s.UserDTO
{
    public class UserDTO
    {
        #region Filed(s)

        public string Username { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        public string Password { get; set; }

        #endregion Filed(s)

        #region Constructor

        public UserDTO() { }

        #endregion Constructor
    }
}
