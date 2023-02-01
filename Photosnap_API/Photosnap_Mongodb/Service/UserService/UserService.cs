using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.UserDTO;
using Photosnap_Mongodb.Enums;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.Service.PhotoService;
using Photosnap_Mongodb.ServiceHelpMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.UserService
{
    public class UserService : IUserService
    {
        private IMongoDatabase _mongoDB;
        private IMongoCollection<User> _userCollection;

        public UserService(IMongoDatabase mongodDatabase)
        {
            _mongoDB = mongodDatabase;
            _userCollection = _mongoDB.GetCollection<User>(PhotosnapCollection.User);
        }

        #region RegisterAndLogin
        
        public async Task<LoginRegisterResponseDTO> CreateUser(UserDTO userBasicInformation)
        {
            try
            {
                if (await HelpMethods.UsernameIsTaken(this._userCollection, userBasicInformation.Username))
                    throw new Exception("Username is already taken.");

                var user = new User();

                user.Username = userBasicInformation.Username;
                user.Biography = userBasicInformation.Biography;
                user.Name = userBasicInformation.Name;
                user.LastName = userBasicInformation.LastName;
                user.Password = PasswordService.EncryptPassword(userBasicInformation.Password, user.PasswordSalt);
                user.ProfilePhotoFilePath = PhotoStoringMethods.WritePhotoToFolder(userBasicInformation.ProfilePhoto, user.Username, PhotoType.UserProfilePhoto);

                var userCollection = this._mongoDB.GetCollection<User>(PhotosnapCollection.User);
                await userCollection.InsertOneAsync(user);

                LoginRegisterResponseDTO responseDTO = new LoginRegisterResponseDTO();
                responseDTO.Username = user.Username;
                responseDTO.ProfilePhoto = PhotoStoringMethods.ReadPhotoFromFile(user.Username, PhotoType.UserProfilePhoto);
                responseDTO.LoginInformation = "Succes";
                return responseDTO;
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex); }
        }

        public async Task<LoginRegisterResponseDTO> Login(UserCredentialsDTO credentials)
        {
            try
            {
                //var user = await HelpMethods.GetUserByUsername(_userCollection, credentials.Username);
                var user = await HelpMethods.GetDocumentByFieldValue<User>(_userCollection, "Username", credentials.Username);
                LoginRegisterResponseDTO response = new LoginRegisterResponseDTO();
                if (user != null)
                {
                    var encryptedPassword = PasswordService.EncryptPassword(credentials.Password, user.PasswordSalt);
                    if (encryptedPassword.SequenceEqual(user.Password))
                    {
                        response.Username = user.Username;
                        response.ProfilePhoto = PhotoStoringMethods.ReadPhotoFromFile(user.Username, PhotoType.UserProfilePhoto);
                        response.LoginInformation = "Succes";
                        return response;
                    }

                    response.LoginInformation = "Ivalid password";
                    return response;
                }
                response.LoginInformation = "Inalid username";
                return response;
            }
            catch (Exception ex) { throw new Exception("Error occured during login procces."); }
        }

        #endregion Create
    }
}
