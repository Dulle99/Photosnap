using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.PhotoCategoryDTO;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
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
                var user = await HelpMethods.GetDocumentByFieldValue(_userCollection, "Username", credentials.Username);
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

        #endregion RegisterAndLogin

        #region Get

        public async Task<List<PhotoDTO>> GetUserPhotos(string username, int numberOfPhotosToGet)
        {
            var photoList = new List<PhotoDTO>();
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", username);

            var photoCollection = this._mongoDB.GetCollection<Photo>(PhotosnapCollection.Photo);
            foreach (var photoId in user.UserPhotos.Take(numberOfPhotosToGet))
            {
                var photo = await HelpMethods.GetDocumentByFieldValue(photoCollection, "PhotoId", photoId.Id);
                photoList.Add(new PhotoDTO
                {
                    PhotoId = photo.PhotoId,
                    Photo = PhotoStoringMethods.ReadPhotoFromFilePath(photo.PhotoFilePath),
                    Description = photo.Description,
                    NumberOfFollowers = user.FollowersOfUser.Count(),
                    NumberOfLikes = photo.PhotoLikes.Count(),
                    NumberOfComments = photo.Comments.Count(),
                    AuthorUsername = user.Username,
                    AuthorProfilePhoto = PhotoStoringMethods.ReadPhotoFromFilePath(user.ProfilePhotoFilePath),
                    CategoryName = photo.Category.CategoryName,
                    CategoryColor = photo.Category.CategoryColor,
                });
            }
            return photoList;
        }

        public async Task<int> GetTotalNumberOfUserPhotos(string username)
        {
            var filteredUser = await HelpMethods.GetSpecificFieldsFromDocument(this._userCollection, "Username", username, new List<string> { "UserPhotos" });
            if (filteredUser != null)
            {
                return filteredUser.UserPhotos.Count;
            }
            else
                return 0;
        }

        public async Task<UserProfilePreviewDTO> GetUserProfilePreview(string username)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", username);
            if (user != null)
            {
                UserProfilePreviewDTO userPreview = new UserProfilePreviewDTO
                {
                    Username = user.Username,
                    Name = user.Name,
                    Lastname = user.LastName,
                    numberOfFollowings = user.FollowingUsers.Count(),
                    NumberOfFollowers = user.FollowersOfUser.Count(),
                    NumberOfCategoriesOfInterst = user.PhotoCategoriesOfInterest.Count(),
                    Biography = user.Biography,
                    ProfilePhoto = PhotoStoringMethods.ReadPhotoFromFile(user.Username, PhotoType.UserProfilePhoto)
                };

                return userPreview;
            }
            else throw new Exception("User not found.");
        }

        public async Task<List<UserChipDTO>> GetUsersListOfFollowing(string username, int numberOfUsersToGet)
        {  
            var filteredUserDocument= await HelpMethods.GetSpecificFieldsFromDocument(this._userCollection, "Username", username, new List<string> { "FollowingUsers" });
            if (filteredUserDocument == null || filteredUserDocument.FollowingUsers == null)
                throw new Exception("Error while retriving list of followings.");

            return await HelpMethods.GetUsersChipPreviews(this._userCollection, filteredUserDocument.FollowingUsers, numberOfUsersToGet);
        }

        public async Task<List<UserChipDTO>> GetUsersListOfFollowers(string username, int numberOfUsersToGet)
        {   
            var filteredUserDocument = await HelpMethods.GetSpecificFieldsFromDocument(this._userCollection, "Username", username, new List<string> { "FollowersOfUser" });
            if (filteredUserDocument == null || filteredUserDocument.FollowingUsers == null)
                throw new Exception("Error while retriving list of followings.");

            return await HelpMethods.GetUsersChipPreviews(this._userCollection, filteredUserDocument.FollowersOfUser, numberOfUsersToGet);
        }


        public async Task<List<PhotoCategoryDTO>> GetUsersListOfPhotoInterests(string username, int numberOfCategoriesToGet)
        {
            var filteredUserDocument = await HelpMethods.GetSpecificFieldsFromDocument(this._userCollection, "Username", username, new List<string> { "PhotoCategoriesOfInterest" });
            if (filteredUserDocument == null || filteredUserDocument.PhotoCategoriesOfInterest == null)
                throw new Exception("Error while retriving list of followings.");

            var photoCategoryList = new List<PhotoCategoryDTO>();
            foreach (var photoCategory in filteredUserDocument.PhotoCategoriesOfInterest.Take(numberOfCategoriesToGet))
                photoCategoryList.Add(new PhotoCategoryDTO { CategoryName = photoCategory.CategoryName, CategoryColor = photoCategory.CategoryColor });
        
            return photoCategoryList;
        }

        #endregion Get

        #region Update

        public async Task FollowUser(string loggedUserUsername, string userUsernameToBeFollowed)
        {
            var loggedUser = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", loggedUserUsername);
            var userToBeFollowed = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", userUsernameToBeFollowed);

            if (loggedUser != null && userToBeFollowed != null)
            {
                if (loggedUser.FollowingUsers.Contains(userToBeFollowed.UserId))
                    throw new Exception("User is already following given user.");

                await HelpMethods.PushValueInFieldCollection(this._userCollection, "UserId", loggedUser.UserId, "FollowingUsers", userToBeFollowed.UserId);

                await HelpMethods.PushValueInFieldCollection(this._userCollection, "UserId", userToBeFollowed.UserId, "FollowersOfUser", loggedUser.UserId);
            }
            else
                throw new Exception("User not found");
        }

        public async Task UnfollowUser(string loggedUserUsername, string userUsernameForUnfollowing)
        {
            var loggedUser = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", loggedUserUsername);
            var userToBeFollowed = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", userUsernameForUnfollowing);

            if (loggedUser != null && userToBeFollowed != null)
            {
                if (!loggedUser.FollowingUsers.Contains(userToBeFollowed.UserId))
                    throw new Exception("User is already unfollowed");

                await HelpMethods.PopValueInFieldCollection(this._userCollection, "UserId", loggedUser.UserId, "FollowingUsers", userToBeFollowed.UserId);

                await HelpMethods.PopValueInFieldCollection(this._userCollection, "UserId", userToBeFollowed.UserId, "FollowersOfUser", loggedUser.UserId);
            }
            else
                throw new Exception("User not found");
        }

        public async Task AddCategoryOfInterest(string userUsername, string categoryName)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", userUsername);

            var categoryCollection = this._mongoDB.GetCollection<PhotoCategory>(PhotosnapCollection.PhotoCategory);
            var photoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", categoryName);

            if (user != null && categoryCollection != null)
            {
                if (user.PhotoCategoriesOfInterest.Exists(category => category.PhotoCategoryId == photoCategory.PhotoCategoryId))
                    throw new Exception("User already have category added to his interests.");

                await HelpMethods.PushValueInFieldCollection(this._userCollection, "UserId", user.UserId, "PhotoCategoriesOfInterest", photoCategory);

            }
            else
                throw new Exception("Objects not found.");
        }

        public async Task RemoveCategoryOfInterest(string userUsername, string categoryName)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", userUsername);

            var categoryCollection = this._mongoDB.GetCollection<PhotoCategory>(PhotosnapCollection.PhotoCategory);
            var photoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", categoryName);

            if (user != null && categoryCollection != null)
            {
                if (!user.PhotoCategoriesOfInterest.Exists(category => category.PhotoCategoryId == photoCategory.PhotoCategoryId)) 
                    throw new Exception("User does not have category added to his interests.");

                await HelpMethods.PopValueInFieldCollection(this._userCollection, "UserId", user.UserId, "PhotoCategoriesOfInterest", photoCategory);
            }
            else
                throw new Exception("Objects not found.");
        }

        #endregion Update

        #region Delete

        
        #endregion Delete
    }
}
