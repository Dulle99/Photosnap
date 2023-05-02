using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.PhotoDTO
{
    public class PhotoUpdateFromInfromationDTO
    {
        #region Field(s)

        public string PhotoAuthor { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public byte[] Photo { get; set; }

        #endregion Field(s)

        #region Constructor

        public PhotoUpdateFromInfromationDTO()
        {

        }

        #endregion Constructor
    }
}
