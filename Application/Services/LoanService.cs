using HelloBuild.Application.Helpers.Interfaces;
using HelloBuild.Application.Services.Interfaces;
using AutoMapper;
using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using System.Net;

namespace HelloBuild.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IValidateUserHelper _userHelper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper, IValidateUserHelper userHelper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        public async Task<Response> Add(LoanRequest request)
        {
            try
            {
                if (request.TipoUsuario == 3)
                {
                    bool userHasLoans = await _userHelper.ValidateGuestUserLoanAsync(request.IdentificacionUsuario);
                    if (userHasLoans)
                    {
                        return new Response
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            IsSuccess = false,
                            Content = new BadResponse { Message = $"El usuario con identificacion {request.IdentificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo" }
                        };
                    }
                }

                Prestamo entity = _mapper.Map<Prestamo>(request);

                await _loanRepository.AddAsync(entity);
                await _loanRepository.SaveChangesAsync();

                return new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Content = new LoanResponse { Id = entity.Id, ReturnDate = entity.MaxDateReturn }
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Content = new BadResponse { Message = ex.Message }
                };
            }
        }

        public async Task<Response> Get(Guid loanId)
        {
            try
            {
                Prestamo? entity = await _loanRepository.GetByIdAsync(loanId);
                if (entity == null)
                {
                    return new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Content = new BadResponse { Message = $"El prestamo con id {loanId} no existe" }
                    };
                }

                LoanUserResponse reponse = _mapper.Map<LoanUserResponse>(entity);

                return new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Content = reponse
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Content = new BadResponse { Message = ex.Message }
                };
            }
        }
    }
}
