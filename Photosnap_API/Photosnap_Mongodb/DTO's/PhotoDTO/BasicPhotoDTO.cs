using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Photosnap_Mongodb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Photosnap_Mongodb.DTO_s.PhotoDTO
{
    public class BasicPhotoDTO
    {
        #region Field(s)

        public string PhotoAuthor { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public IFormFile Photo { get; set; }

        #endregion Field(s)

        #region Constructor

        public BasicPhotoDTO()
        {

        }

        #endregion Constructor
    }
}

