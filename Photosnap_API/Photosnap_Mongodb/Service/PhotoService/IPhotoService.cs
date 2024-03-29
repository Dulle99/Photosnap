﻿using MongoDB.Bson;
using Photosnap_Mongodb.DTO_s.CommentDTO;
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
        public Task AddComment(BasicCommentDTO comment);
        public Task<bool> DeletePhoto(string photoId);

        public Task<PhotoUpdateFromInfromationDTO>GetPhotoUpdateInformation(string photoId);
        public Task<List<PhotoDTO>> GetPhotosByCategories(string[] categoriesName, int numberOfPhotosToGet);
        public Task<List<CommentPreviewDTO>> GetPhotoComments(string photoId, int numberOfCommentsToGet);

        public Task EditPhoto(EditPhotoDTO photoDTO);
        public Task<int> LikePhotoButton(string userUsername, string photoId);
        public Task<int> UnlikePhoto(string userUsername, string photoId);
       
    }
}
