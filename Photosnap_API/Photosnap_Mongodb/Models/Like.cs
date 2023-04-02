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
    public class Like
    {
        #region Field(s)

        [BsonId]
        public ObjectId LikeId { get; set; }

        public ObjectId UserId { get; set; }

        public ObjectId PhotoId { get; set; }

        public DateTime Timestamp {get; set;}

        #endregion Field(s)

        #region Constructor(s)

        public Like() { Timestamp = DateTime.UtcNow; }

        public Like(ObjectId userId, ObjectId photoId)
        {
            Timestamp = DateTime.UtcNow;
            UserId = userId;   
            PhotoId = photoId;
        }

        #endregion Constructor(s)

    }
}
