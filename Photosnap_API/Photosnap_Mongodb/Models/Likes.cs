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
    public class Likes
    {
        [BsonId]
        public ObjectId LikeId { get; set; }

        public ObjectId UserId { get; set; }

        public ObjectId PhotoId { get; set; }

        public DateTime Timestamp {get; set;}

        public Likes() { }

    }
}
