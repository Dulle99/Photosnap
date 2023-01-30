using Photosnap_Mongodb.DTO_s.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.UserService
{
    public interface IUserService
    {
        public Task<LoginRegisterResponseDTO> CreateUser(UserDTO userBasicInformation);
        public Task<LoginRegisterResponseDTO> Login(UserCredentialsDTO credentials);
    }
}
