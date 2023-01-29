using Photosnap_Mongodb.DTO_s.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.UserService
{
    public interface IUserService
    {
        public Task CreateUser(UserDTO userBasicInformation);
    }
}
