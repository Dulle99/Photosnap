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
    public class Photo
    {
        #region Field(s)

        [BsonId]
        public ObjectId PhotoId { get; set; }

        public string Description { get; set; }

        public string PhotoFilePath { get; set; }

        public DateTime PublicationDate { get; set; }

        public int NumberOfLikes { get; set; }

        //TODO Pitati profesira da li je dobra praksa da kreiram posebnu kolekciju tipa AuthorBasic sa osnovnim informacijama i da onda to emebdujem.
        public User AuthorOfThePhoto { get; set; }

        public PhotoCategory Category { get; set; }

        public List<Comment> Comments { get; set; }

        #endregion Field(s)

        #region Constructor

        public Photo() 
        {
            PublicationDate = DateTime.Now;
            Comments = new List<Comment>();
        }

        #endregion Constructor
    }
}
