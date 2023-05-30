using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.CommentDTO
{
    public class CommentPreviewDTO
    {
        #region Field(s)

        public string CommentContent { get; set; }

        public string AuthorOfCommentUsername { get; set; }

        public byte[] AuthorOfCommentProfilePhoto { get; set; }

        public string CommentId { get; set; }

        #endregion Field(s)

        #region Constructor

        public CommentPreviewDTO() { }

        #endregion Constructor
    }
}
