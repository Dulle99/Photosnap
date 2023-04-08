using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.UserDTO
{
    public class UserChipDTO
    {
        #region Filed(s)

        public string Username { get; set; }

        public byte[] ProfilePhoto { get; set; }

        #endregion Filed(s)

        #region Constructor

        public UserChipDTO() { }

        #endregion Constructor
    }
}
