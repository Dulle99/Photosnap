using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.CommentDTO
{
    public class BasicCommentDTO
    {
        #region Field(s)

        public string CommentContent { get; set; }

        public string AuthorOfCommentUsername { get; set; }

        public string PhotoId { get; set; }

        #endregion Field(s)

        #region Constructor

        public BasicCommentDTO() { }

        #endregion Constructor
    }
}
