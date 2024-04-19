using Hydra.BusinessLayer.Repository.IService.IAccountService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hydra.BusinessLayer.Repository.Service.AccountService
{
    public class AccountService(IUnitOfWork unitOfWork, IConfiguration configuration) : EncryptionService, IAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;


        public async Task<ApiResponse> Register(UserRegisterModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower())).FirstOrDefaultAsync();
            if (user != null)
                return new(400, ResponseConstants.UserNameExists);

            user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                MobileNumber = model.MobileNumber,
                Password = Encipher(model.Password),
                IsActive = true,
                IsApproved = false,
                AccessLevelId = model.AccessLevelId,
                DepartmentId = model.DepartmentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            user.UserRole.Add(new()
            {
                RoleId = (long)Roles.Staff,
            });

            await _unitOfWork.UserRepository.Create(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Success);
        }


        public async Task<ServiceResponse<LoginResponse>> Login(LoginModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) && x.IsActive && x.IsApproved)
                                                       .Include(x => x.UserRole).ThenInclude(x => x.Role)
                                                       .FirstOrDefaultAsync();
            if (user == null)
                return new(400, ResponseConstants.InvalidUserName);
            
            if (Encipher(model.Password) != user.Password)
                return new(400, ResponseConstants.InvalidPassword);

            return new(200, ResponseConstants.Success, new LoginResponse
            {
                AccessToken = AccessToken(user, user.UserRole.FirstOrDefault().Role.Name, user?.UserRole?.FirstOrDefault()?.RoleId),
            });
        }


        public async Task<ApiResponse> ResetPassword(PasswordResetModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) && x.IsActive && x.IsApproved).FirstOrDefaultAsync();
            if (user == null)
                return new(400, ResponseConstants.InvalidUserName);

            user.Password = Encipher(model.Password);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Password);
        }


        #region Account Service Helpers

        protected string AccessToken(User appUser, string roles, long? roleId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtOptions:SecurityKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.GetValue<string>("JwtOptions:Issuer"),
                Audience = _configuration.GetValue<string>("JwtOptions:Audience"),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Email, appUser.Email),
                    new(ClaimTypes.NameIdentifier, $"{appUser.Id}"),
                    new("userName",appUser.UserName),
                    new("roles", roles),
                    new("roleId", roleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
