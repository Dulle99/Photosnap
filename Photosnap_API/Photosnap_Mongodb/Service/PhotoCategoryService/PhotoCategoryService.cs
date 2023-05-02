using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.PhotoCategoryDTO;
using Photosnap_Mongodb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.PhotoCategoryService
{
    public class PhotoCategoryService : IPhotoCategoryService
    {
        private IMongoDatabase _db;
        private IMongoCollection<PhotoCategory> _photoCategoriesCollection;

        public PhotoCategoryService(IMongoDatabase mongoDatabase)
        {
            _db = mongoDatabase;
            _photoCategoriesCollection = _db.GetCollection<PhotoCategory>(PhotosnapCollections.PhotoCategory);
        }


        public async Task<List<PhotoCategoryDTO>> GetPhotoCategories()
        {
            var photoCategoriesDTOs = new List<PhotoCategoryDTO>();
            var filter = Builders<PhotoCategory>.Filter.Empty;
            var result = await this._photoCategoriesCollection.FindAsync(filter);
            var photoCategories = result.ToList();
            foreach(var photoCategory in photoCategories ) 
                photoCategoriesDTOs.Add(new PhotoCategoryDTO(photoCategory.CategoryName, photoCategory.CategoryColor));

            return photoCategoriesDTOs;
            
        }

        public async Task CreateCategory(PhotoCategoryDTO photoCategoryDTO)
        {
            try
            {
                var category = new PhotoCategory();
                category.CategoryName = photoCategoryDTO.CategoryName;
                category.CategoryColor = photoCategoryDTO.CategoryColor;
                await this._photoCategoriesCollection.InsertOneAsync(category);
            }
            catch(Exception ex) { }
        }

        public async Task<bool> RemoveCategory(string categoryName)
        {
            try
            {
                var filter = Builders<PhotoCategory>.Filter.Eq("CategoryName", categoryName);
                var result = await this._photoCategoriesCollection.FindOneAndDeleteAsync(filter);
                if(result !=null)
                    return true;
                return false;
            }
            catch (Exception ex) { return false; }
        }
    }
}
