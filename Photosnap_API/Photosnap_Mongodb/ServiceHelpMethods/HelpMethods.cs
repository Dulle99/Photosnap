using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
using Photosnap_Mongodb.DTO_s.UserDTO;
using Photosnap_Mongodb.Models;
using Photosnap_Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Photosnap_Mongodb.ServiceHelpMethods
{
    public static class HelpMethods
    {
        #region Get

        public async static Task<bool> UsernameIsTaken(IMongoCollection<User> userCollection, string username)
        {
            if (await GetDocumentByFieldValue(userCollection, "Username", username) != null)
                return true;

            return false;
        }

        public async static Task<TDocument> GetDocumentByFieldValue<TDocument, TValue>(IMongoCollection<TDocument> collection, string fieldName, TValue fieldValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(fieldName, fieldValue);
            var cursor = await collection.FindAsync(filter);
            return cursor.FirstOrDefault();
        }

        public async static Task<TDocument> GetSpecificFieldsFromDocument<TDocument, TFilterValueType>(IMongoCollection<TDocument> collection, string filterFieldName,
                                                                                                       TFilterValueType filterFieldValue, List<string> fieldNames)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterFieldName, filterFieldValue);
            var projectionList = new List<ProjectionDefinition<TDocument>>();
            foreach(var fieldName in fieldNames)
            {
                var projection = Builders<TDocument>.Projection.Include(fieldName);
                projectionList.Add(projection);
            }

            var combinedProjection = Builders<TDocument>.Projection;
            var options = new FindOptions<TDocument> { Projection = combinedProjection.Combine(projectionList) };
            var cursor = await collection.FindAsync(filter, options);
            return cursor.FirstOrDefault();
        }

        public static async Task<List<UserChipDTO>> GetUsersChipPreviews(IMongoCollection<User> userCollection, List<ObjectId> userIds, int numberOfUsersToGet)
        {
            List<UserChipDTO> userChipDTOs = new List<UserChipDTO>();
            foreach (var userId in userIds.Take(numberOfUsersToGet))
            {
                var filteredUser = await HelpMethods.GetSpecificFieldsFromDocument(userCollection, "UserId", userId, new List<string> { "Username", "ProfilePhotoFilePath" });
                if (filteredUser != null || filteredUser.FollowingUsers != null || filteredUser.ProfilePhotoFilePath != null)
                {
                    userChipDTOs.Add(new UserChipDTO
                    {
                        Username = filteredUser.Username,
                        ProfilePhoto = PhotoStoringMethods.ReadPhotoFromFilePath(filteredUser.ProfilePhotoFilePath)
                    });
                }
            }
            return userChipDTOs;
        }

        #endregion Get

        #region Update

        public static async Task UpdateFieldInCollecton<TDocument, TFilterFieldVal, TUpdateFieldValue>(IMongoCollection<TDocument> collection, string filterCollectionIdentifierName, TFilterFieldVal filterCollectionIdentifierValue,
                                                                string updateFieldName, TUpdateFieldValue updateFieldValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterCollectionIdentifierName, filterCollectionIdentifierValue);
            var update = Builders<TDocument>.Update.Set(updateFieldName, updateFieldValue);
            await collection.UpdateOneAsync(filter, update);
        }

        public static async Task PushValueInFieldCollection<TDocument, TFilterFieldVal, TUpdateFieldValue>(IMongoCollection<TDocument> collection, string filterCollectionIdentifierName, TFilterFieldVal filterCollectionIdentifierValue,
                                                                 string updateListFieldName, TUpdateFieldValue updateFieldValue)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterCollectionIdentifierName, filterCollectionIdentifierValue);
            var update = Builders<TDocument>.Update.Push(updateListFieldName, updateFieldValue);
            await collection.UpdateOneAsync(filter, update);
        }

        public static async Task PopValueInFieldCollection<TDocument, TFilterFieldVal, TUpdateFieldValue>(IMongoCollection<TDocument> collection, string filterCollectionIdentifierName, TFilterFieldVal filterCollectionIdentifierValue,
                                                                string ListFieldName, TUpdateFieldValue FieldValueToPop)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterCollectionIdentifierName, filterCollectionIdentifierValue);
            var update = Builders<TDocument>.Update.Pull(ListFieldName, FieldValueToPop);
            await collection.UpdateOneAsync(filter, update);
        }

        #endregion Update

        #region Delete

        public static async Task<bool> RemoveDocument<TDocument>(IMongoCollection<TDocument> collection, string filterFieldName, ObjectId documentId)
        {
            var filter = Builders<TDocument>.Filter.Eq(filterFieldName, documentId);
            var deleteResult = await collection.DeleteOneAsync(filter);
            if (deleteResult.IsAcknowledged)
                return true;
            return false;
        }

        public async static Task DeleteCommentsOfPhoto(IMongoDatabase db, Photo photo)
        {
            var commentCollection = db.GetCollection<Comment>(PhotosnapCollections.Comment);
            foreach (var comment in photo.Comments)
            {
                await RemoveDocument(commentCollection, "CommentId", comment.CommentId);
            }
        }

        public async static Task DeletePhotoLikes(IMongoDatabase db, Photo photo)
        {
            var userCollection = db.GetCollection<User>(PhotosnapCollections.User);
            var likeCollection = db.GetCollection<Like>(PhotosnapCollections.Like);
            foreach (var likeId in photo.PhotoLikes)
            {
                var like = await GetDocumentByFieldValue(likeCollection, "LikeId", likeId);
                await PopValueInFieldCollection(userCollection, "UserId", like.UserId, "UserLikes", like.LikeId);
                await RemoveDocument(likeCollection, "LikeId", likeId);
            }
        }

        #endregion Delete
    }
}
