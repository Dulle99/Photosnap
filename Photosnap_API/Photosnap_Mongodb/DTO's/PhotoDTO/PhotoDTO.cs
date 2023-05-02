using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.PhotoDTO
{
    public class PhotoDTO
    {
        #region Field(s)

        public string PhotoId { get; set; }

        public byte[] Photo { get; set; }

        public string Description { get; set; }

        public int NumberOfFollowers { get; set; }

        public int NumberOfLikes { get; set; }

        public int NumberOfComments { get; set; }

        public string AuthorUsername { get; set; }

        public byte[] AuthorProfilePhoto { get; set; }

        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }

        #endregion Field(s)

        #region Constructor

        public PhotoDTO() { }

        #endregion Constructor
    }
}
