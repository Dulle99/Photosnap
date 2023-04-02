using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.CommentDTO;
using Photosnap_Mongodb.DTO_s.PhotoDTO;
using Photosnap_Mongodb.Service.PhotoService;

namespace Photosnap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private IPhotoService _photoService;

        public PhotoController(IMongoDatabase db)
        {
            _photoService = new PhotoService(db);
        }

        [HttpPost]
        [Route("PostPhoto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostPhoto([FromForm]BasicPhotoDTO basicPhotoDTO)
        {
            try
            {
                await this._photoService.CreatePhoto(basicPhotoDTO);
                return Ok();
            }
            catch (Exception ex) 
            {

                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddComment([FromForm] BasicCommentDTO comment)
        {
            try
            {
               await this._photoService.AddComment(comment);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("LikePhoto/{userUsername}/{photoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LikePhoto(string userUsername, string photoId)
        {
            try
            {
                await this._photoService.LikePhoto(userUsername, photoId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UnlikePhoto/{userUsername}/{photoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnlikePhoto(string userUsername, string photoId)
        {
            try
            {
                await this._photoService.UnlikePhoto(userUsername, photoId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeletePhoto/{photoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePhoto(string photoId)
        {
            try
            {
                await this._photoService.DeletePhoto(photoId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

    }
}
