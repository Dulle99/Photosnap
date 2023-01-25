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

        public Guid PhotoId { get; set; }

        public string Description { get; set; }

        public string PhotoFilePath { get; set; }

        public DateTime PublicationDate { get; set; }

        public MongoDBRef AuthorOfThePhoto { get; set; }

        public MongoDBRef Category { get; set; }

        public List<MongoDBRef> Comments { get; set; }

        #endregion Field(s)

        #region Constructor

        public Photo() 
        {
            PhotoId = Guid.NewGuid();
            PublicationDate = DateTime.Now;
            Comments = new List<MongoDBRef>();
        }

        #endregion Constructor
    }
}
