using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.ServiceHelpMethods;
using System;
using System.Collections.Generic;
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
            //TODO: photo.FilePath

            await this._photoCollection.InsertOneAsync(photo);

            return true;
        }
    }
}
