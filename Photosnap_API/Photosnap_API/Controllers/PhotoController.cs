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

        [HttpPut]
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

        [HttpPut]
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
