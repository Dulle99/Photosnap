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
    public class PhotoCategory
    {
        #region Field(s)

        [BsonId]
        public ObjectId PhotoCategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }

        public List<MongoDBRef> Photos { get; set; }

        #endregion Field(s)

        #region Constructor

        public PhotoCategory()
        {
            Photos = new List<MongoDBRef>();
        }  

        #endregion Constructor
    }
}
