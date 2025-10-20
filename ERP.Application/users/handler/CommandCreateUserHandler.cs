using AutoMapper;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared._base.BaseMessage;
using ERP.Shared._base.BaseResponse;
using ERP.Shared.authenticationservice;
using ERP.Shared.encryptservice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users.handler
{
    public class CommandCreateUserHandler : IRequestHandler<CommandCreateUser, BaseResponse<UserDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        private readonly IPasswordHashingService _passwordHashing;
        public CommandCreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId, IPasswordHashingService passwordHashing)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
            _passwordHashing = passwordHashing;
        }
        public async Task<BaseResponse<UserDTO>> Handle(CommandCreateUser request, CancellationToken cancellationToken)
        {
            var result = new BaseResponse<UserDTO>();
            try
            {
                return await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var user = await CreateUser(request.Args, request.CurrentUser);
                    if(user != null)
                    {
                        var userMapper = _mapper.Map<User, UserDTO>(user);
                        userMapper.UserId = _hashId.EncodeId(user.UserId);
                        userMapper.CompanyId = user.CompanyId != null ? _hashId.EncodeId(user.CompanyId.GetValueOrDefault()) : null;
                        userMapper.BranchId = user.BranchId != null ? _hashId.EncodeId(user.BranchId.GetValueOrDefault()) : null;
                        userMapper.Created_By_Id = _hashId.EncodeId(user.Created_By_Id);
                        userMapper.Last_Update_By_Id = _hashId.EncodeId(user.Last_Update_By_Id);
                        result.Data = userMapper;
                        result.Message = BaseMessage.dataSuccess;
                        result.IsSuccess = true;
                    }

                    return result;
                });
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<User?> CreateUser(CreateUserDTO args, CurrentUserVM currentUser)
        {
            DateTime now = DateTime.UtcNow;
            if (!await _unitOfWork.Users.AnyAsync(f => f.UserName == args.UserName))
            {
                var userMapper = _mapper.Map<CreateUserDTO, User>(args);
                userMapper.Password = _passwordHashing.HashPassword(args.Password);

                userMapper.CompanyId = _hashId.DecodeToInt(args.CompanyId);
                userMapper.BranchId = _hashId.DecodeToInt(args.BranchId);

                userMapper.IsActive = true;
                userMapper.IsDelete = false;

                userMapper.Created_By_Id = currentUser.UserId;
                userMapper.Creation_Date = now;

                userMapper.Last_Update_By_Id = currentUser.UserId;
                userMapper.Last_Update_By_Date = now;

                await _unitOfWork.Users.AddAsync(userMapper);
                await _unitOfWork.SaveChangesAsync();

                return userMapper;
            }
            else
            {
                throw new Exception(string.Format(BaseMessage.existsUserMsg, args.UserName));
            }
        }
    }
}
