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

        public Task FollowUser(string loggedUserUsername, string userUsernameToBeFollowed);
        public Task UnfollowUser(string loggedUserUsername, string userUsernameForUnfollowing);
        public Task AddCategoryOfInterest(string userUsername, string  categoryName);
        public Task RemoveCategoryOfInterest(string userUsername, string categoryName);

        public Task<UserProfilePreviewDTO> GetUserProfilePreview(string username);
    }
}
