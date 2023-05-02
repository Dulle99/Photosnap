using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.PhotoDTO
{
    public class EditPhotoDTO
    {
        #region Field(s)

        public string PhotoId { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        #endregion Field(s)

        #region Constructor

        public EditPhotoDTO()
        {

        }

        #endregion Constructor
    }
}
