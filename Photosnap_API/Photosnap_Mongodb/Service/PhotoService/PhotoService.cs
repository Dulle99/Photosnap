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
            _photoCollection = _mongoDB.GetCollection<Photo>(PhotosnapCollection.Photo);
        }

        public async Task<bool> CreatePhoto(BasicPhotoDTO basicPhoto)
        {

            var userCollection = _mongoDB.GetCollection<User>(PhotosnapCollection.User);
            var categoryCollection = _mongoDB.GetCollection<PhotoCategory>(PhotosnapCollection.PhotoCategory);

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

            var photosByCategory = photoCategory.Photos;
            photosByCategory.Add(new MongoDBRef(PhotosnapCollection.Photo,photo.PhotoId));
            await HelpMethods.UpdateFieldInCollecton(categoryCollection, "PhotoCategoryId", photoCategory.PhotoCategoryId, "Photos", photosByCategory);

            var userPhotos = author.UserPhotos;
            userPhotos.Add(new MongoDBRef(PhotosnapCollection.Photo, photo.PhotoId));
            await HelpMethods.UpdateFieldInCollecton(userCollection, "UserId", author.UserId, "UserPhotos", userPhotos);

            return true;
        }

        public async Task AddComment(BasicCommentDTO commentDTO)
        {
            try
            {
                var _commentService = _mongoDB.GetCollection<Comment>(PhotosnapCollection.Comment);
                var userCollection = _mongoDB.GetCollection<User>(PhotosnapCollection.User);
                Comment comment = new Comment();
                comment.CommentContent = commentDTO.CommentContent;
                comment.AssociatedToPhoto = new MongoDBRef(PhotosnapCollection.Photo, new ObjectId(commentDTO.PhotoId));
                comment.AuthorOfComment = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", commentDTO.AuthorOfCommentUsername);
                _commentService.InsertOne(comment);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<bool> DeletePhoto(string photoId)
        {
            ObjectId _photoId = new ObjectId(photoId);
            var userCollection = _mongoDB.GetCollection<User>(PhotosnapCollection.User);
            var categoryCollection = _mongoDB.GetCollection<PhotoCategory>(PhotosnapCollection.PhotoCategory);
            var _commentService = _mongoDB.GetCollection<Comment>(PhotosnapCollection.Comment);

            var photo = await HelpMethods.GetDocumentByFieldValue(this._photoCollection, "PhotoId", _photoId);
            var photoCategory = await HelpMethods.GetDocumentByFieldValue(categoryCollection, "CategoryName", photo.Category.CategoryName);
            var author = await HelpMethods.GetDocumentByFieldValue(userCollection, "Username", photo.AuthorOfThePhoto.Username);
            if (photo == null || author == null || photoCategory == null)
                return false;

            var photosByCategory = photoCategory.Photos;
            photosByCategory = photosByCategory.Where(dbRef => dbRef.Id != _photoId).ToList();
            await HelpMethods.UpdateFieldInCollecton(categoryCollection, "PhotoCategoryId", photoCategory.PhotoCategoryId, "Photos", photosByCategory);

            var userPhotos = author.UserPhotos;
            userPhotos = photosByCategory.Where(dbRef => dbRef.Id != _photoId).ToList();
            await HelpMethods.UpdateFieldInCollecton(userCollection, "UserId", author.UserId, "UserPhotos", userPhotos);


            //TODO: remove comments associated to photo
            

            PhotoStoringMethods.DeletePhotoFromFolder(photoId.ToString(), PhotoType.ContentPhoto);
            return await HelpMethods.RemoveDocument(_photoCollection, "PhotoId", _photoId);
        }
    }
}
