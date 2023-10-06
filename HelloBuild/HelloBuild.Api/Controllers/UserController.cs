using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HelloBuild.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Save new user.
        /// </summary>
        /// <param name="isbn">Book ID.</param>
        /// <param name="identificaciónUsuario">User ID</param>
        /// <param name="tipoUsuario">User Type</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("SaveUser")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(BadResponse), 400)]
        public async Task<IActionResult> Post(UserRequest request)
        {
            Response userResponse = await _userService.AddUser(request);

            return userResponse.StatusCode == HttpStatusCode.BadRequest
                ? (IActionResult)BadRequest(userResponse.Content)
                : userResponse.StatusCode == HttpStatusCode.InternalServerError
                ? StatusCode(StatusCodes.Status500InternalServerError, userResponse.Content)
                : (IActionResult)Ok(userResponse.Content);
        }

        /// <summary>
        /// Is User registered?
        /// </summary>
        /// <param name="id_prestamo">Loan ID.</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("UserRegistered")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(BadResponse), 404)]
        public async Task<IActionResult> Get(UserExistRequest request)
        {
            Response response = await _userService.UserExist(request);

            return response.StatusCode == HttpStatusCode.NotFound
                ? (IActionResult)NotFound(response.Content)
                : response.StatusCode == HttpStatusCode.InternalServerError
                ? (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, response.Content)
                : (IActionResult)Ok(response.Content);
        }

    }
}
