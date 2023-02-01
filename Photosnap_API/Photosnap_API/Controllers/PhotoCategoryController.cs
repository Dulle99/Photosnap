using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Photosnap_Mongodb.DTO_s.PhotoCategoryDTO;
using Photosnap_Mongodb.Service.PhotoCategoryService;

namespace Photosnap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoCategoryController : ControllerBase
    {
        private IPhotoCategoryService _photoCategoryService;

        public PhotoCategoryController(IMongoDatabase db) 
        {
            _photoCategoryService = new PhotoCategoryService(db);
        }

        [HttpPost]
        [Route("AddPhotoCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPhotoCategory([FromForm]PhotoCategoryDTO photoCategoryDTO)
        {
            try
            {
                await this._photoCategoryService.CreateCategory(photoCategoryDTO);
                return Ok();
            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetPhotoCategories")]
        public async Task<IActionResult> GetPhotoCategories()
        {
            return new JsonResult(await this._photoCategoryService.GetPhotoCategories()); 
        }

        [HttpDelete]
        [Route("RemovePhotoCategory/{photoCategoryDTO}")]
        public async Task<IActionResult> RemovePhotoCategory(string photoCategoryDTO)
        {
            var result = await this._photoCategoryService.RemoveCategory(photoCategoryDTO);
            if (result)
                return Ok();
            return BadRequest();
        }
    }
}
