using Photosnap_Mongodb.DTO_s.PhotoCategoryDTO;
using Photosnap_Mongodb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.PhotoCategoryService
{
    public interface IPhotoCategoryService
    {
        public Task<List<PhotoCategoryDTO>> GetPhotoCategories();
        public Task CreateCategory(PhotoCategoryDTO photoCategoryDTO);
        public Task<bool> RemoveCategory(string categoryName);
    }
}
