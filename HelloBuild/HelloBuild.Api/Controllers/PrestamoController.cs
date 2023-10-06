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
    public class PrestamoController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public PrestamoController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        /// <summary>
        /// Save new loan by user.
        /// </summary>
        /// <param name="isbn">Book ID.</param>
        /// <param name="identificaciónUsuario">User ID</param>
        /// <param name="tipoUsuario">User Type</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [ProducesResponseType(typeof(LoanResponse), 200)]
        [ProducesResponseType(typeof(BadResponse), 400)]
        public async Task<IActionResult> Post(LoanRequest request)
        {
            Response loanResponse = await _loanService.Add(request);

            return loanResponse.StatusCode == HttpStatusCode.BadRequest
                ? (IActionResult)BadRequest(loanResponse.Content)
                : loanResponse.StatusCode == HttpStatusCode.InternalServerError
                ? StatusCode(StatusCodes.Status500InternalServerError, loanResponse.Content)
                : (IActionResult)Ok(loanResponse.Content);
        }

        /// <summary>
        /// Get a specific loan using Loan ID
        /// </summary>
        /// <param name="id_prestamo">Loan ID.</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("{id_prestamo}")]
        [ProducesResponseType(typeof(LoanUserResponse), 200)]
        [ProducesResponseType(typeof(BadResponse), 404)]
        public async Task<IActionResult> Get(Guid id_prestamo)
        {
            Response getResponse = await _loanService.Get(id_prestamo);
            return getResponse.StatusCode == HttpStatusCode.NotFound
                ? (IActionResult)NotFound(getResponse.Content)
                : getResponse.StatusCode == HttpStatusCode.InternalServerError
                ? (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, getResponse.Content)
                : (IActionResult)Ok(getResponse.Content);
        }

    }
}
