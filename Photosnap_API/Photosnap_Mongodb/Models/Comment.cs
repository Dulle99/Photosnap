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
    public class Comment
    {
        #region Field(s)

        [BsonId]
        public ObjectId CommentId { get; set; }

        public string CommentContent { get; set; }

        public DateTime PublicationDate { get; set; }

        public User AuthorOfComment { get; set; }

        public MongoDBRef AssociatedToPhoto { get; set; }

        #endregion Field(s)

        #region Constructor

        public Comment() 
        {
            PublicationDate = DateTime.Now;
        }

        #endregion Constructor
    }
}
