using MongoDB.Bson;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.Service.PhotoService
{
    public interface IPhotoService
    {
        public Task<bool> CreatePhoto(BasicPhotoDTO photo);
        public Task<bool> DeletePhoto(string photoId);
    }
}
