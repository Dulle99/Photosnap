using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Photosnap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IMongoDatabase _database;
        public UserController(IMongoDatabase database)
        {
            _database= database;
        }
    }
}
