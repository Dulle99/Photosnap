using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Models
{
    public class Follow
    {
        [BsonId]
        public ObjectId FollowId { get; set; }    

        public ObjectId UserId { get; set; }

        public ObjectId FollowingUserId { get; set; }

        public DateTime Timestamp { get; set; }

        public Follow() { }
    }
}
