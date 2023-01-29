using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.User;
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

        public UserService(IMongoDatabase mongodDatabase)
        {
            _mongoDB = mongodDatabase;
        }

        #region Create
        
        public async Task CreateUser(UserDTO userBasicInformation)
        {
            try
            {
                if (await HelpMethods.UsernameIsTaken(this._mongoDB, userBasicInformation.Username))
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

            }
            catch (Exception ex) { throw new Exception(ex.Message, ex); }
        }


        #endregion Create
    }
}
