using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseMessage;
using ERP.Shared._base.BaseResponse;
using ERP.Shared.authenticationservice;
using ERP.Shared.encryptservice;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ERP.Application.users.handler
{
    public class CommandLoginHandler : IRequestHandler<CommandLogin, BaseResponse<LoginResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashIdService _hashId;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHashingService _passwordHashing;
        private readonly IConfiguration _configuration;

        public CommandLoginHandler(
            IUnitOfWork unitOfWork,
            IHashIdService hashId,
            IJwtService jwtService,
            IPasswordHashingService passwordHashing,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _hashId = hashId;
            _jwtService = jwtService;
            _passwordHashing = passwordHashing;
            _configuration = configuration;
        }

        public async Task<BaseResponse<LoginResponseDTO>> Handle(CommandLogin request, CancellationToken cancellationToken)
        {
            var result = new BaseResponse<LoginResponseDTO>();
            try
            {
                // Find user by username
                var user = await _unitOfWork.Users
                    .GetAsync(u => u.UserName == request.Args.UserName && u.IsActive && !u.IsDelete);

                if (user == null)
                {
                    return new BaseResponse<LoginResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password",
                        Data = null
                    };
                }

                // Verify password
                bool isPasswordValid = _passwordHashing.VerifyPassword(request.Args.Password, user.Password);

                if (!isPasswordValid)
                {
                    return new BaseResponse<LoginResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password",
                        Data = null
                    };
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(user.UserId, user.UserName, user.CompanyId, user.BranchId);
                var expiryMinutes = _configuration.GetValue<int>("Jwt:ExpiryMinutes", 60);

                // Create response
                var loginResponse = new LoginResponseDTO
                {
                    UserId = _hashId.EncodeId(user.UserId),
                    UserName = user.UserName,
                    CompanyId = user.CompanyId.HasValue ? _hashId.EncodeId(user.CompanyId.Value) : null,
                    BranchId = user.BranchId.HasValue ? _hashId.EncodeId(user.BranchId.Value) : null,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
                };

                result.Data = loginResponse;
                result.Message = "Login successful";
                result.IsSuccess = true;

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponseDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
