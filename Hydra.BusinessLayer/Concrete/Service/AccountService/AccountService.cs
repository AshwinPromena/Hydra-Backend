﻿using Hydra.BusinessLayer.Repository.IService.IAccountService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                IsApproved = false,
                AccessLevelId = model.AccessLevelId,
                DepartmentId = model.DepartmentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                UniversityId = model.UniversityId,
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

            if (Encipher(model.Password)!= user.Password)
                return new(400, ResponseConstants.InvalidPassword);

            if (user.UserRole.FirstOrDefault().Role.LoginType != model.LoginType)
                return new(400, ResponseConstants.ApplicationForbiddenError);

            return new(200, ResponseConstants.Success, new LoginResponse
            {
                AccessToken = AccessToken(user),
            });
        }

        public async Task<ApiResponse> ForgotPassword(ForgotPasswordModel model)
        {
            var Token = Guid.NewGuid().ToString();
            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) &&
                                       x.IsActive && x.IsApproved)
                                        .Include(x => x.Verification)
                                        .Include(x => x.PasswordResetToken)
                                        .Include(x => x.UserRole)
                                        .ThenInclude(x => x.Role)
                                        .FirstOrDefaultAsync();
            if (user == null)
                return new(400, ResponseConstants.InvalidUserName);

            if (user.UserRole.FirstOrDefault().Role.LoginType != model.LoginType)
                return new(400, ResponseConstants.LoginTypeMismatchError);

            try
            {
                var token = new PasswordResetToken
                {
                    UserId = user.Id,
                    ResetToken = Token,
                    IsTokenActive = true,
                    TokenExpiryDate = DateTime.UtcNow.AddMinutes(10),
                    CreatedDate = DateTime.UtcNow,
                };
                await _emailService.SendPasswordResetLink(user.Email, user.Id, user.UserName, Token);
                await _unitOfWork.PasswordResetTokenRepository.Create(token);
                await _unitOfWork.UserRepository.CommitChanges();

                return new(200, ResponseConstants.PasswordResetOtpSent);
            }
            catch (Exception ex)
            {
                return new(400, ex.Message);
            }
        }

        public async Task<ServiceResponse<string>> ValidateResetUrl(string token)
        {
            var user = await _unitOfWork.PasswordResetTokenRepository
                                               .FindByCondition(x =>
                                               x.ResetToken == token &&
                                              x.IsTokenActive == true && x.User.Id == x.UserId)
                                               .Include(i => i.User)
                                               .FirstOrDefaultAsync();

            if (user == null)
                return new(400, ResponseConstants.InvalidToken);

            if (user.TokenExpiryDate <= DateTime.UtcNow)
                return new(400, ResponseConstants.TokenExpired);

            var Otp = await _emailService.SendPasswordResetOTP(user.User.Email, user.User.UserName);
            var userAuthentication = user.User.Verification.FirstOrDefault() == null ? new() : user.User.Verification.FirstOrDefault();
            userAuthentication.OTP = Otp;
            userAuthentication.OtpExpiryDate = DateTime.UtcNow.AddMinutes(5);
            userAuthentication.UserId = user.User.Id;
            userAuthentication.IsActive = true;
            user.IsTokenActive = false;
            var userToken = user.IsTokenActive == false ? Guid.NewGuid().ToString() : user.ResetToken;
            user.IsTokenActive = true;
            user.TokenExpiryDate = DateTime.UtcNow.AddMinutes(10);
            user.UserId = user.User.Id;
            _unitOfWork.VerificationRepository.Update(userAuthentication);
            _unitOfWork.PasswordResetTokenRepository.Update(user);
            await _unitOfWork.VerificationRepository.CommitChanges();


            return new(200, ResponseConstants.Success, user.ResetToken);
        }

        public async Task<ServiceResponse<string>> ValidateOtp(ValidateOtpModel model)
        {
            var token = Guid.NewGuid().ToString();
            var user = await _unitOfWork.PasswordResetTokenRepository
                                        .FindByCondition(x => x.ResetToken == model.Token &&
                                       x.IsTokenActive)
                                        .Include(i => i.User)
                                        .FirstOrDefaultAsync();

            if (user == null)
                return new(400, ResponseConstants.InvalidToken);

            var userVerification = await _unitOfWork.VerificationRepository
                                                    .FindByCondition(x => x.OTP == model.Otp &&
                                                   x.IsActive).FirstOrDefaultAsync();

            if (userVerification == null)
                return new(400, ResponseConstants.InvalidOtp);

            if (userVerification.OtpExpiryDate <= DateTime.UtcNow)
                return new(400, ResponseConstants.OtpExpired);

            userVerification.IsActive = false;

            user.ResetToken = token;
            user.IsTokenActive = true;
            _unitOfWork.PasswordResetTokenRepository.Update(user);
            _unitOfWork.VerificationRepository.Update(userVerification);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Success, token);
        }

        public async Task<ApiResponse> ResetPassword(ResetPasswordModel model)
        {
            var userPasswordResetToken = await _unitOfWork.PasswordResetTokenRepository
                                        .FindByCondition(x => x.ResetToken == model.Token &&
                                                              x.UserId == x.User.Id &&
                                                              x.IsTokenActive)
                                        .Include(i => i.User)
                                        .FirstOrDefaultAsync();
            if (userPasswordResetToken == null)
                return new(400, ResponseConstants.InvalidToken);

            userPasswordResetToken.User.Password = Encipher(model.Password);
            userPasswordResetToken.IsTokenActive = false;

            _unitOfWork.UserRepository.Update(userPasswordResetToken.User);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Password);
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
                    new("name",$"{appUser.FirstName} {appUser.LastName}"),
                    new("roles", $"{appUser.UserRole.FirstOrDefault().Role.Name}"),
                    new("roleId", $"{appUser.UserRole.FirstOrDefault().RoleId}"),
                    new("accessLevelId",$"{appUser.AccessLevelId}"),
                    new("accessLevel", $"{appUser.AccessLevel.Name}"),
                    new("departmentId", $"{appUser.DepartmentId}"),
                    new("department",$"{appUser.Department?.Name}"),
                    new("profilePicture",$"{appUser.ProfilePicture}"),
                    new("universityId", $"{appUser.UniversityId}")
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
