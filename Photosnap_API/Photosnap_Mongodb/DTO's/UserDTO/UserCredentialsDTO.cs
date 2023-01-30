using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.UserDTO
{
    public class UserCredentialsDTO
    {
        #region Filed(s)

        public string Username { get; set; }

        public string Password { get; set; }

        #endregion Filed(s)

        #region Constructor

        public UserCredentialsDTO() { }

        #endregion Constructor
    }
}
