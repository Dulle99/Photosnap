using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.PhotoCategoryDTO;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
using Photosnap_Mongodb.DTO_s.UserDTO;
using Photosnap_Mongodb.Enums;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.ServiceHelpMethods;

namespace Photosnap_Mongodb.Service.UserService
{
    public class UserService : IUserService
    {
        private IMongoDatabase _mongoDB;
        private IMongoCollection<User> _userCollection;

        public UserService(IMongoDatabase mongodDatabase)
        {
            _mongoDB = mongodDatabase;
            _userCollection = _mongoDB.GetCollection<User>(PhotosnapCollections.User);
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

                var userCollection = this._mongoDB.GetCollection<User>(PhotosnapCollections.User);
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

        public async Task<bool> IsUserFollowed(string loggedUserUsername, string username)
        {
            var loggedUser = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", loggedUserUsername);
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", username);

            if(loggedUser == null || user == null) { return false; }

            var result = loggedUser.FollowingUsers.Contains(user.UserId);
            return result;
        }

        public async Task<List<PhotoDTO>> GetUserPhotos(string username, int numberOfPhotosToGet)
        {
            var photoList = new List<PhotoDTO>();
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", username);

            var photoCollection = this._mongoDB.GetCollection<Photo>(PhotosnapCollections.Photo);
            foreach (var photoId in user.UserPhotos.Take(numberOfPhotosToGet))
            {
                var photo = await HelpMethods.GetDocumentByFieldValue(photoCollection, "PhotoId", photoId.Id);
                photoList.Add(new PhotoDTO
                {
                    PhotoId = photo.PhotoId.ToString(),
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

        public async Task<List<PhotoDTO>> GetUserLikedPhotos(string username, int numberOfPhotosToGet)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", username);
            var likesCollection = this._mongoDB.GetCollection<Like>(PhotosnapCollections.Like);
            var photoCollection = this._mongoDB.GetCollection<Photo>(PhotosnapCollections.Photo);

            var photoList = new List<PhotoDTO>();
            foreach(var like_id in user.UserLikes) 
            {
                var like = await HelpMethods.GetDocumentByFieldValue(likesCollection, "LikeId", like_id);
                var photo = await HelpMethods.GetDocumentByFieldValue(photoCollection, "PhotoId", like.PhotoId);
                photoList.Add(new PhotoDTO
                {
                    PhotoId = photo.PhotoId.ToString(),
                    Photo = PhotoStoringMethods.ReadPhotoFromFilePath(photo.PhotoFilePath),
                    Description = photo.Description,
                    NumberOfFollowers = photo.AuthorOfThePhoto.FollowersOfUser.Count(),
                    NumberOfLikes = photo.PhotoLikes.Count(),
                    NumberOfComments = photo.Comments.Count(),
                    AuthorUsername = photo.AuthorOfThePhoto.Username,
                    AuthorProfilePhoto = PhotoStoringMethods.ReadPhotoFromFilePath(photo.AuthorOfThePhoto.ProfilePhotoFilePath),
                    CategoryName = photo.Category.CategoryName,
                    CategoryColor = photo.Category.CategoryColor,
                });
            }

            return photoList;
        }

        public async Task<List<PhotoDTO>> GetPhotosOfFollowingUsers(string username, int numberOfPhotosToGet)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", username);
            var listOfFollowingUsers = new List<User>();
            listOfFollowingUsers.Add(user);
            foreach (var userId in user.FollowingUsers)
            {
                listOfFollowingUsers.Add(await HelpMethods.GetDocumentByFieldValue(this._userCollection, "UserId", userId));
            }

            var photoCollection = this._mongoDB.GetCollection<Photo>(PhotosnapCollections.Photo);
            var photos = new List<Photo>();

            foreach (var followingUser in listOfFollowingUsers)
            {
                var followingUser_LatestPhotos = followingUser.UserPhotos.Take(5);
                foreach (var photoId in followingUser_LatestPhotos)
                    photos.Add(await HelpMethods.GetDocumentByFieldValue(photoCollection, "PhotoId", photoId.Id));
            }
            photos = photos.OrderByDescending(photo => photo.PublicationDate).ToList();

            var photosDTO = new List<PhotoDTO>();
            foreach(var photo in photos.Take(numberOfPhotosToGet))
            {
                photosDTO.Add(new PhotoDTO
                {
                    PhotoId = photo.PhotoId.ToString(),
                    Photo = PhotoStoringMethods.ReadPhotoFromFilePath(photo.PhotoFilePath),
                    Description = photo.Description,
                    NumberOfFollowers = photo.AuthorOfThePhoto.FollowersOfUser.Count(),
                    NumberOfLikes = photo.PhotoLikes.Count(),
                    NumberOfComments = photo.Comments.Count(),
                    AuthorUsername = photo.AuthorOfThePhoto.Username,
                    AuthorProfilePhoto = PhotoStoringMethods.ReadPhotoFromFilePath(photo.AuthorOfThePhoto.ProfilePhotoFilePath),
                    CategoryName = photo.Category.CategoryName,
                    CategoryColor = photo.Category.CategoryColor,
                });
            }
            return photosDTO;
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

        public async Task<int> GetTotalNumberOfUserLikedPhotos(string username)
        {
            var filteredUser = await HelpMethods.GetSpecificFieldsFromDocument(this._userCollection, "Username", username, new List<string> { "UserLikes" });
            if (filteredUser != null)
            {
                return filteredUser.UserLikes.Count;
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
            var filteredUserDocument = await HelpMethods.GetSpecificFieldsFromDocument(this._userCollection, "Username", username, new List<string> { "FollowingUsers" });
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

        public async Task AddCategoryOfInterest(string userUsername, List<string> categoryNames)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", userUsername);
            var categoryCollection = this._mongoDB.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);

            if (user != null && categoryCollection != null)
            {
                foreach(var category in categoryNames)
                {
                    var photoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", category);
                    if (photoCategory != null)
                        if (user.PhotoCategoriesOfInterest.Exists(category => category.PhotoCategoryId == photoCategory.PhotoCategoryId))
                            continue;
                        else
                            await HelpMethods.PushValueInFieldCollection(this._userCollection, "UserId", user.UserId,
                                                                         "PhotoCategoriesOfInterest", photoCategory);
                }
            }
            else
                throw new Exception("Objects not found.");
        }

        public async Task RemoveCategoryOfInterest(string userUsername, string categoryName)
        {
            var user = await HelpMethods.GetDocumentByFieldValue(this._userCollection, "Username", userUsername);

            var categoryCollection = this._mongoDB.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);
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
