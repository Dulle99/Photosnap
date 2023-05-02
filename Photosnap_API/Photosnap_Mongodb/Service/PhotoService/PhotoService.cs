using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.CommentDTO;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
using Photosnap_Mongodb.Enums;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.ServiceHelpMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.PhotoService
{
    public class PhotoService : IPhotoService
    {
        private IMongoDatabase _mongoDB;
        private IMongoCollection<Photo> _photoCollection;

        public PhotoService(IMongoDatabase mongodDatabase) 
        { 
            _mongoDB= mongodDatabase;
            _photoCollection = _mongoDB.GetCollection<Photo>(PhotosnapCollections.Photo);
        }

        #region Create

        public async Task<bool> CreatePhoto(BasicPhotoDTO basicPhoto)
        {

            var userCollection = _mongoDB.GetCollection<User>(PhotosnapCollections.User);
            var categoryCollection = _mongoDB.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);

            var author = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", basicPhoto.PhotoAuthor);
            var photoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", basicPhoto.Category);
            if (author == null || photoCategory == null)
                return false;

            var photo = new Photo();
            photo.AuthorOfThePhoto = author;
            photo.Category = photoCategory;
            photo.Description = basicPhoto.Description;
            await _photoCollection.InsertOneAsync(photo);


            var photoFilePath = PhotoStoringMethods.WritePhotoToFolder(basicPhoto.Photo, photo.PhotoId.ToString(), Enums.PhotoType.ContentPhoto);
            await HelpMethods.UpdateFieldInCollecton(_photoCollection, "PhotoId", photo.PhotoId, "PhotoFilePath", photoFilePath);

            await HelpMethods.PushValueInFieldCollection(categoryCollection, "PhotoCategoryId", photoCategory.PhotoCategoryId, "Photos", new MongoDBRef(PhotosnapCollections.Photo, photo.PhotoId));
            await HelpMethods.PushValueInFieldCollection(userCollection, "UserId", author.UserId, "UserPhotos", new MongoDBRef(PhotosnapCollections.Photo, photo.PhotoId));

            return true;
        }

        public async Task AddComment(BasicCommentDTO commentDTO)
        {
            try
            {
                var _commentService = _mongoDB.GetCollection<Comment>(PhotosnapCollections.Comment);
                var userCollection = _mongoDB.GetCollection<User>(PhotosnapCollections.User);
                Comment comment = new Comment();
                comment.CommentContent = commentDTO.CommentContent;
                comment.AssociatedToPhoto = new MongoDBRef(PhotosnapCollections.Photo, new ObjectId(commentDTO.PhotoId));
                comment.AuthorOfComment = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", commentDTO.AuthorOfCommentUsername);
                _commentService.InsertOne(comment);

                await HelpMethods.PushValueInFieldCollection(this._photoCollection,"PhotoId", new ObjectId(commentDTO.PhotoId), "Comments", comment);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        #endregion Create

        #region Get

        public async Task<PhotoUpdateFromInfromationDTO> GetPhotoUpdateInformation(string photoId)
        {
            ObjectId objectId = new ObjectId(photoId);
            var photo = await HelpMethods.GetDocumentByFieldValue(this._photoCollection, "PhotoId", objectId);
            if (photo != null)
            {
                return new PhotoUpdateFromInfromationDTO
                {
                    PhotoAuthor = photo.AuthorOfThePhoto.Username,
                    Category = photo.Category.CategoryName,
                    Description = photo.Description,
                    Photo = PhotoStoringMethods.ReadPhotoFromFilePath(photo.PhotoFilePath)
                };
            }
            return new PhotoUpdateFromInfromationDTO();


        }

        #endregion Get

        #region Update

        public async Task EditPhoto(EditPhotoDTO photoDTO)
        {
            var photoId = new ObjectId(photoDTO.PhotoId);
            var photo = await HelpMethods.GetDocumentByFieldValue(this._photoCollection, "PhotoId", photoId);
            if(photo != null) 
            {
                await HelpMethods.UpdateFieldInCollecton(this._photoCollection, "PhotoId", photoId, "Description", photoDTO.Description);
                if(photo.Category.CategoryName != photoDTO.Category)
                {
                    var categoryCollection = this._mongoDB.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);
                    await HelpMethods.PopValueInFieldCollection(categoryCollection, "PhotoCategoryId", photo.Category.PhotoCategoryId, "Photos", new MongoDBRef(PhotosnapCollections.Photo, photo.PhotoId));

                    var newPhotoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", photoDTO.Category);
                    await HelpMethods.UpdateFieldInCollecton(this._photoCollection, "PhotoId", photoId, "Category", newPhotoCategory);
                    await HelpMethods.PushValueInFieldCollection(categoryCollection, "CategoryName", photoDTO.Category, "Photos", new MongoDBRef(PhotosnapCollections.Photo, photo.PhotoId));
                }
            }

        }

        public async Task LikePhoto(string userUsername, string photoId)
        {
            var userCollection = this._mongoDB.GetCollection<User>(PhotosnapCollections.User);
            var user = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", userUsername);

            var photo = await HelpMethods.GetDocumentByFieldValue(this._photoCollection, "PhotoId", new ObjectId(photoId)); 

            if (user != null && photo != null)
            {
                var likeCollection = this._mongoDB.GetCollection<Like>(PhotosnapCollections.Like);
                var like = new Like(user.UserId, photo.PhotoId);
                await likeCollection.InsertOneAsync(like);

                await HelpMethods.PushValueInFieldCollection(userCollection, "UserId", user.UserId, "UserLikes", like.LikeId);
                await HelpMethods.PushValueInFieldCollection(this._photoCollection, "PhotoId", photo.PhotoId, "PhotoLikes", like.LikeId);
            }
            else
                throw new Exception("Objects not found");
        }


        public async Task UnlikePhoto(string userUsername, string photoId)
        {
            var userCollection = this._mongoDB.GetCollection<User>(PhotosnapCollections.User);
            var user = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", userUsername);

            var photo = await HelpMethods.GetDocumentByFieldValue(this._photoCollection, "PhotoId",new ObjectId(photoId));

            if (user != null && photo != null)
            {
                var likeCollection = this._mongoDB.GetCollection<Like>(PhotosnapCollections.Like);
                var like = await HelpMethods.GetDocumentByFieldValue(likeCollection, "UserId", user.UserId);

                await HelpMethods.PopValueInFieldCollection(userCollection, "UserId", user.UserId, "UserLikes", like.LikeId);
                await HelpMethods.PopValueInFieldCollection(this._photoCollection, "PhotoId", photo.PhotoId, "PhotoLikes", like.LikeId);

                await HelpMethods.RemoveDocument(likeCollection, "LikeId", like.LikeId);
            }
            else
                throw new Exception("Objects not found");
        }

        #endregion Update

        #region Delete

        public async Task<bool> DeletePhoto(string photoId)
        {
            ObjectId _photoId = new ObjectId(photoId);
            var userCollection = _mongoDB.GetCollection<User>(PhotosnapCollections.User);
            var categoryCollection = _mongoDB.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);

            var photo = await HelpMethods.GetDocumentByFieldValue(this._photoCollection, "PhotoId", _photoId);
            var photoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", photo.Category.CategoryName);
            var author = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", photo.AuthorOfThePhoto.Username);
            if (photo == null || author == null || photoCategory == null)
                return false;

            await HelpMethods.PopValueInFieldCollection(categoryCollection, "PhotoCategoryId", photoCategory.PhotoCategoryId, "Photos", new MongoDBRef(PhotosnapCollections.Photo, photo.PhotoId)); 
            await HelpMethods.PopValueInFieldCollection(userCollection, "UserId", author.UserId, "UserPhotos", new MongoDBRef(PhotosnapCollections.Photo, photo.PhotoId)); 

            await HelpMethods.DeleteCommentsOfPhoto(this._mongoDB, photo);
            await HelpMethods.DeletePhotoLikes(this._mongoDB, photo);

            PhotoStoringMethods.DeletePhotoFromFolder(photoId.ToString(), PhotoType.ContentPhoto);
            return await HelpMethods.RemoveDocument(_photoCollection, "PhotoId", _photoId);
        }

        #endregion Delete
    }
}
