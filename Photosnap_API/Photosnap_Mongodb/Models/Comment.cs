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

        public Guid CommentId { get; set; }

        public string CommentContent { get; set; }

        public DateTime PublicationDate { get; set; }

        #endregion Field(s)

        #region Constructor

        public Comment() 
        {
            CommentId = Guid.NewGuid();
            PublicationDate = DateTime.Now;
        }

        #endregion Constructor
    }
}
