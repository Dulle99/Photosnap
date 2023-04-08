using Microsoft.AspNetCore.Http;
using Photosnap_Mongodb.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.ServiceHelpMethods
{
    public static class PhotoStoringMethods
    {
        public static string WritePhotoToFolder(IFormFile ImageFile, string photoId, PhotoType photoType )
        {
            byte[] imageBinary = null;
            string folderPath = GetFolderPathByPhotoType(photoType);

            string photoFilePath = "";
            if (ImageFile.Length > 0)
            {
                using (var binaryReader = new BinaryReader(ImageFile.OpenReadStream()))
                {
                    imageBinary = binaryReader.ReadBytes((int)ImageFile.Length);
                }
                photoFilePath = folderPath + photoId;
                File.WriteAllBytes(photoFilePath, imageBinary);
            }
            return photoFilePath;
        }

        public static byte[] ReadPhotoFromFile(string photoId, PhotoType photoType)
        {
                string folderPath = GetFolderPathByPhotoType(photoType);
                string photoFilePath = folderPath + photoId; //+ ".jpg";
                return ReadPhotoFromFilePath(photoFilePath);
        }

        public static byte[] ReadPhotoFromFilePath(string filePath)
        {
            try
            {
                byte[] photo = File.ReadAllBytes(filePath);
                if (photo.Length > 0)
                    return photo;
                else
                    return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                return Array.Empty<byte>();
            }
        }

        public static void DeletePhotoFromFolder(string photoName, PhotoType photoType)
        {
            string folderPath = GetFolderPathByPhotoType(photoType);
            string photoFilePath = folderPath + photoName;
            File.Delete(photoFilePath);
        }

        public static string GetFolderPathByPhotoType(PhotoType phototype)
        {
            string folderPath = "";
            if (phototype == PhotoType.ContentPhoto)
                folderPath = "C:\\Users\\DusanSotirov\\Desktop\\NBP Projects\\Photosnap_photos\\ContentPhotos\\";
            else
                folderPath = "C:\\Users\\DusanSotirov\\Desktop\\NBP Projects\\Photosnap_photos\\ProfilePhotos\\";
            return folderPath;
        }
    }
}
