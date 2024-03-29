﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Photosnap_API.Jwt;
using Photosnap_Mongodb.DTO_s.UserDTO;
using Photosnap_Mongodb.Service.UserService;

namespace Photosnap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IMongoDatabase database)
        {
            _userService= new UserService(database);
        }

        [HttpPost]
        [Route("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromForm] UserDTO userBasicDTO)
        {
            try
            {
                var response = await _userService.CreateUser(userBasicDTO);
                response.Token = JwtToken.GenerateToken(response.Username);
                return new JsonResult(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromForm] UserCredentialsDTO credentials)
        {
            try
            {
                var loginResult = await _userService.Login(credentials);
                if (loginResult.Username == credentials.Username)
                {
                    loginResult.Token = JwtToken.GenerateToken(loginResult.Username);
                    return new JsonResult(loginResult);
                }
                return BadRequest(loginResult.LoginInformation);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("Follow/{username}/{usernameToBeFollowed}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Follow(string username, string usernameToBeFollowed)
        {
            try
            {
               await _userService.FollowUser(username, usernameToBeFollowed);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("Unfollow/{username}/{usernameToBeUnfollowed}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Unfollow(string username, string usernameToBeUnfollowed)
        {
            try
            {
                await _userService.UnfollowUser(username, usernameToBeUnfollowed);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("AddCategoryOfInterest/{username}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategoryOfInterest([FromQuery] List<string> categoryNames, string username)
        {
            try
            {
                await _userService.AddCategoryOfInterest(username, categoryNames);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("RemoveCategoryOfInterest/{username}/{categoryName}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveCategoryOfInterest(string username, string categoryName)
        {
            try
            {
                await _userService.RemoveCategoryOfInterest(username, categoryName);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("IsUserFollowed/{loggedUserUsername}/{userUsername}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserFollowed(string loggedUserUsername, string userUsername)
        {
            try
            {
                return new JsonResult(await this._userService.IsUserFollowed(loggedUserUsername, userUsername));

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetUserPhotos/{username}/{numberOfPhotosToGet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserPhotos(string username, int numberOfPhotosToGet)
        {
            try
            {
                return new JsonResult(await this._userService.GetUserPhotos(username, numberOfPhotosToGet));

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetUserLikedPhotos/{username}/{numberOfPhotosToGet}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserLikedPhotos(string username, int numberOfPhotosToGet)
        {
            try
            {
                return new JsonResult(await this._userService.GetUserLikedPhotos(username, numberOfPhotosToGet));

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetPhotosOfFollowingUsers/{username}/{numberOfPhotosToGet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPhotosOfFollowingUsers(string username, int numberOfPhotosToGet)
        {
            try
            {
                return new JsonResult(await this._userService.GetPhotosOfFollowingUsers(username, numberOfPhotosToGet));

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpGet]
        [Route("GetTotalNumberOfUserPhotos/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalNumberOfPhotos(string username)
        {
            try
            {
                return new JsonResult(await this._userService.GetTotalNumberOfUserPhotos(username));

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetTotalNumberOfUserLikedPhotos/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTotalNumberOfLikedPhotos(string username)
        {
            try
            {
                return new JsonResult(await this._userService.GetTotalNumberOfUserLikedPhotos(username)); ;

            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetUserProfilePreview/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserProfilePreview(string username)
        {
            try
            {
                return new JsonResult(await this._userService.GetUserProfilePreview(username));

            }
            catch(Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetUserListOfFollwings/{username}/{numberOfUsersToGet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserListOfFollwings(string username, int numberOfUsersToGet)
        {
            try
            {
                return new JsonResult(await this._userService.GetUsersListOfFollowing(username, numberOfUsersToGet));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetUserListOfFollwers/{username}/{numberOfUsersToGet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserListOfFollwers(string username, int numberOfUsersToGet)
        {
            try
            {
                return new JsonResult(await this._userService.GetUsersListOfFollowers(username, numberOfUsersToGet));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet]
        [Route("GetUserListOfPhotoInterests/{username}/{numberOfUsersToGet}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserListOfPhotoInterests(string username, int numberOfUsersToGet)
        {
            try
            {
                return new JsonResult(await this._userService.GetUsersListOfPhotoInterests(username, numberOfUsersToGet));
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
