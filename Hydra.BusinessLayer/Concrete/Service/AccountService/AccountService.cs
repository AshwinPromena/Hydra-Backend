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
    public class AccountService(HydraContext context, IConfiguration configuration, IUnitOfWork unitOfWork) : IAccountService
    {
        private readonly HydraContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task<ApiResponse> Register(UserModel model)
        {
            var verifyUser = await _unitOfWork.UserRepository.FindByCondition(x => x.Email == model.Email)
                                                .Include(x => x.Department)
                                                .Include(x => x.AccessLevel)
                                                .Include(x => x.UserRole)
                                                .ThenInclude(x => x.Role).FirstOrDefaultAsync();
            if (verifyUser == null)
            {
                return new(400, ResponseConstants.Exists);
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                MobileNumber = model.MobileNumber,
            };
            user.UserRole.Add(new()
            {
                UserId = user.Id,
                RoleId = (long)Roles.Staff,
            });
            user.Password = model.Password;

            _context.Add(user);
            await _context.SaveChangesAsync();

            return new(200, ResponseConstants.Success);
        }


        public async Task<ServiceResponse<LoginResponse>> Login(LoginModel model)
        {
            var verifuUser = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName == model.UserName && x.IsActive == true)
                                                .Include(x => x.UserRole).ThenInclude(x => x.Role).FirstOrDefaultAsync();
            if (verifuUser == null)
            {
                return new(404, ResponseConstants.InvalidCredential);
            }
            if (model.Password == EncryptionService.Decipher(verifuUser.Password))
            {
                return new(404, ResponseConstants.InvalidCredential);
            }
            var accessToken = AccessToken(verifuUser, verifuUser?.UserRole?.FirstOrDefault()?.Role?.Name, verifuUser?.UserRole?.FirstOrDefault()?.RoleId);

            return new(200, ResponseConstants.Success, new LoginResponse
            {
                AccessToken = accessToken,
            });
        }


        public async Task<ApiResponse> ResetPassword(PasswordResetModel model)
        {
            var verifyUser = await _unitOfWork.UserRepository.FindByCondition(x => x.Email == model.Email).FirstOrDefaultAsync();
            verifyUser.Password = EncryptionService.Encipher(model.NewPassword);
            _context.Update(verifyUser);
            await _context.SaveChangesAsync();

            return new(200, ResponseConstants.Password);
        }


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
                    new("roles", roles),
                    new("roleId", roleId.ToString())
                }),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
