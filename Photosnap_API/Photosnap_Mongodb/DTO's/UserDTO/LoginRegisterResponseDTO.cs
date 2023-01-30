using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.DTO_s.UserDTO
{

    public class LoginRegisterResponseDTO
    {
        #region Fields

        public string Username { get; set; }

        public string Token { get; set; }

        public string LoginInformation { get; set; }

        public byte[] ProfilePhoto { get; set; }

        #region Fields

        #endregion Constructor

        public LoginRegisterResponseDTO()
        {
            Username = string.Empty;
            Token = string.Empty;
            LoginInformation = string.Empty;
        }

        #endregion Constructor
    }
}
