﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Models
{
    public class Photo
    {
        #region Field(s)

        [BsonId]
        public ObjectId PhotoId { get; set; }

        public string Description { get; set; }

        public string PhotoFilePath { get; set; }

        public DateTime PublicationDate { get; set; }

        public User AuthorOfThePhoto { get; set; }

        public PhotoCategory Category { get; set; }

        public List<Comment> Comments { get; set; }

        public List<ObjectId> PhotoLikes { get; set; }

        #endregion Field(s)

        #region Constructor

        public Photo() 
        {
            PhotoLikes = new List<ObjectId>();  
            PublicationDate = DateTime.Now;
            Comments = new List<Comment>();
        }

        #endregion Constructor
    }
}
