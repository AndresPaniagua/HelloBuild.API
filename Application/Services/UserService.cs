using AutoMapper;
using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Entities;
using HelloBuild.Domain.Models;
using HelloBuild.Infrastructure.Repositories.Interfaces;
using System.Net;

namespace HelloBuild.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Response> AddUser(UserRequest request)
        {
            try
            {
                User entity = _mapper.Map<User>(request);

                await _userRepository.AddAsync(entity);
                int savedChanges = await _userRepository.SaveChangesAsync();

                return savedChanges <= 0
                    ? new Response
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        Content = new BadResponse { Message = $"Could not save user correctly" }
                    }
                    : new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccess = true,
                        Content = new UserResponse { Message = "User successfully added", IsSave = true }
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

        public async Task<Response> UserExist(UserExistRequest request)
        {
            try
            {
                bool exists = await _userRepository.FinduserEmailPasswordAsync(request);
                return !exists
                    ? new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Content = new BadResponse { Message = $"The user {request.Email} does not exist." }
                    }
                    : new Response
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccess = true,
                        Content = new UserResponse { Message = "User exist", Exists = true }
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

        public async Task<Response> GetUserInfo(GetUserInfoRequest request)
        {
            try
            {
                User entity = await _userRepository.FinduserEmailAsync(request.Email);
                if (entity == null)
                {
                    return new Response
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccess = false,
                        Content = new BadResponse { Message = $"The user {request.Email} does not exist." }
                    };
                }

                GetUserInfoResponse response = _mapper.Map<GetUserInfoResponse>(entity);

                return new Response
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Content = response
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
