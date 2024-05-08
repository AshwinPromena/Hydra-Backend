using DocumentFormat.OpenXml.Spreadsheet;
using Hydra.BusinessLayer.Repository.IService.IAccountService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Hydra.BusinessLayer.Repository.Service.AccountService
{
    public class AccountService(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService) : EncryptionService, IAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IConfiguration _configuration = configuration;

        private readonly IEmailService _emailService = emailService;

        public async Task<ApiResponse> Register(UserRegisterModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower())).FirstOrDefaultAsync();
            if (user != null)
                return new(400, ResponseConstants.UserNameExists);

            user = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                MobileNumber = model.MobileNumber,
                Password = Encipher(model.Password),
                IsActive = true,
                IsApproved = true,
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
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()))
                                                       .Include(x => x.UserRole).ThenInclude(x => x.Role)
                                                       .Include(i => i.AccessLevel)
                                                       .Include(i => i.Department)
                                                       .FirstOrDefaultAsync();
            if (user == null)
                return new(400, ResponseConstants.InvalidUserName);
            if (!user.IsActive)
                return new(400, ResponseConstants.AccountInactive);
            if (!user.IsApproved)
                return new(400, ResponseConstants.NotApproved);

            if (Encipher(model.Password) != user.Password)
                return new(400, ResponseConstants.InvalidPassword);

            return new(200, ResponseConstants.Success, new LoginResponse
            {
                AccessToken = AccessToken(user),
            });
        }

        public async Task<ApiResponse> ForgotPassword(ForgotPasswordModel model)
        {
            var Token = GenerateRefereshToken();
            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) &&
                                       x.IsActive && x.IsApproved)
                                        .Include(x => x.Verification)
                                        .FirstOrDefaultAsync();
            if (user == null)
                return new(400, ResponseConstants.InvalidUserName);
            try
            {
                var userVerification = user.Verification.FirstOrDefault() == null ? new() : user.Verification.FirstOrDefault();
                userVerification.UserId = user.Id;
                userVerification.PasswordResetToken = Token;
                userVerification.IsTokenActive = true;
                userVerification.PasswordResetTokenExpiryDate = DateTime.UtcNow.AddMinutes(10);
                userVerification.CreatedDate = DateTime.UtcNow;
                await _emailService.SendPasswordResetLink(user.Email, user.Id, user.UserName, Token);
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.VerificationRepository.Update(userVerification);
                await _unitOfWork.UserRepository.CommitChanges();

                return new(200, ResponseConstants.PasswordResetOtpSent);
            }
            catch (Exception ex)
            {
                return new(400, ex.Message);
            }
        }

        public async Task<ServiceResponse<string>> ValidateResetUrl(long userId, string token)
        {
            var user = await _unitOfWork.UserRepository
                                               .FindByCondition(x => x.Id == userId &&
                                               x.Verification.FirstOrDefault().PasswordResetToken == token)
                                               .Include(i => i.Verification)
                                               .FirstOrDefaultAsync();

            if (user == null)
                return new(400, ResponseConstants.InvalidToken);

            if (user.Verification.FirstOrDefault().PasswordResetTokenExpiryDate <= DateTime.UtcNow)
                return new(400, ResponseConstants.TokenExpired);

            var Otp = await _emailService.SendPasswordResetOTP(user.Email, user.UserName);
            var userVerification = user.Verification.FirstOrDefault() == null ? new() : user.Verification.FirstOrDefault();
            userVerification.OTP = Otp;
            userVerification.OtpExpiryDate = DateTime.UtcNow.AddMinutes(10);
            _unitOfWork.VerificationRepository.Update(userVerification);
            await _unitOfWork.VerificationRepository.CommitChanges();

            return new(200, ResponseConstants.Success, userVerification.PasswordResetToken);
        }

        public async Task<ApiResponse> ResetPassword(ResetPasswordModel model)
        {
            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.Verification.FirstOrDefault().UserId == x.Id &&
                                       x.Verification.FirstOrDefault().PasswordResetToken == model.Token)
                                        .Include(i => i.Verification)
                                        .FirstOrDefaultAsync();

            if (user == null)
                return new(400, ResponseConstants.InvalidToken);

            if (user.Verification.FirstOrDefault().OTP != model.Otp)
                return new(400, ResponseConstants.InvalidOtp);

            if (user.Verification.FirstOrDefault().OtpExpiryDate <= DateTime.UtcNow)
                return new(400, ResponseConstants.OtpExpired);

            user.Password = Encipher(model.Password);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Password);
        }

        public async Task<ApiResponse> ReSendOtp(ForgotPasswordModel model)
        {
            var Token = Guid.NewGuid().ToString();
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) && x.IsActive && x.IsApproved).FirstOrDefaultAsync();
            if (user == null)
                return new(400, ResponseConstants.InvalidUserName);
            try
            {
                user.Verification.Add(new()
                {
                    UserId = user.Id,
                    OTP = await _emailService.SendPasswordResetOTP(user.Email, user.UserName),
                    OtpExpiryDate = DateTime.UtcNow.AddMinutes(10),
                    PasswordResetToken = Token,
                    IsTokenActive = true,
                    CreatedDate = DateTime.UtcNow,
                });

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.UserRepository.CommitChanges();

                return new(200, ResponseConstants.PasswordResetOtpSent);
            }
            catch (Exception ex)
            {
                return new(400, ex.Message);
            }
        }


        #region Account Service Helpers

        protected string AccessToken(User appUser)
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
                    new("roles", $"{appUser.UserRole.FirstOrDefault().Role.Name}"),
                    new("roleId", $"{appUser.UserRole.FirstOrDefault().RoleId}"),
                    new("accessLevelId",$"{appUser.AccessLevelId}"),
                    new("accessLevel", $"{appUser.AccessLevel.Name}"),
                    new("departmentId", $"{appUser.DepartmentId}"),
                    new("department",$"{appUser.Department?.Name}"),
                    new("profilePicture",$"{appUser.ProfilePicture}")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefereshToken()
        {
#pragma warning disable SYSLIB0023 
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
#pragma warning restore SYSLIB0023 
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        #endregion
    }
}
