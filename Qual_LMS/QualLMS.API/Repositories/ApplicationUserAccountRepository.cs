using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QualLMS.API.Data;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.API.Repositories
{
    public class ApplicationUserAccountRepository(DataContext context, CustomLogger logger, IConfiguration config) : IApplicationUserAccount
    {
        public ServiceResponse.ResponsesWithData AllUsers(Guid OrganizationId)
        {
            try
            {
                var result = context.ApplicationUser.Where(u => u.OrganizationId == OrganizationId).Select(user => new UserAllData
                {
                    Id = user.Id,
                    EmailId = user.EmailId!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = user.Role.ToString()
                }).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", ex.Message);
            }
        }

        public ServiceResponse.GeneralResponses CreateAccount(UserRegister model)
        {
            try
            {
                var data = context.ApplicationUser.FirstOrDefault(o => o.Id == model.Id);
                if (data == null)
                {
                    var Model = new ApplicationUser()
                    {
                        EmailId = model.EmailId,
                        CreatedAt = DateTime.Now,
                        FullName = model.FullName,
                        OrganizationId = model.OrganizationId,
                        ParentName = model.ParentName,
                        ParentNumber = model.ParentNumber,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        Role = model.RoleId,
                    };
                    context.ApplicationUser.Add(Model);
                }
                else
                {
                    data.EmailId = model.EmailId;
                    data.ModifiedAt = logger.CurrentDateTime(DateTime.Now);
                    data.FullName = model.FullName;
                    data.ParentName = model.ParentName;
                    data.ParentNumber = model.ParentNumber;
                    data.PhoneNumber = model.PhoneNumber;
                }
                context.SaveChanges();

                return new GeneralResponses(true, "Data Updated!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                string Message = "Error Occured!" + ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("IX_ApplicationUser_EmailId"))
                    {
                        Message = "Error Occured::Email Id already Exists";
                    }
                    else
                    {
                        Message += "<br/>" + ex.InnerException.Message;
                    }
                }
                return new GeneralResponses(false, Message);
            }
        }

        public ServiceResponse.ResponsesWithData GetStudents(string OrgId)
        {
            try
            {
                var result = context.ApplicationUser.Where(u => u.OrganizationId == new Guid(OrgId) && u.Role == Roles.Students).Select(user => new UserAllData
                {
                    Id = user.Id,
                    EmailId = user.EmailId!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = user.Role.ToString()
                }).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", ex.Message);
            }
        }

        public ServiceResponse.ResponsesWithData GetTeachers(string OrgId)
        {
            try
            {
                var result = context.ApplicationUser.Where(u => u.OrganizationId == new Guid(OrgId) && u.Role == Roles.Teachers).Select(user => new UserAllData
                {
                    Id = user.Id,
                    EmailId = user.EmailId!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = user.Role.ToString()
                }).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", ex.Message);
            }
        }

        public ServiceResponse.ResponsesWithData GetUser(string Id)
        {
            try
            {
                var result = context.ApplicationUser.Select(user => new UserAllData
                {
                    Id = user.Id,
                    EmailId = user.EmailId!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = user.Role.ToString()
                }).FirstOrDefault(u => u.Id == new Guid(Id));

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", ex.Message);
            }
        }

        public ServiceResponse.LoginResponses LoginAccount(Login user)
        {
            try
            {
                LoginProperties properties = new LoginProperties();
                if (user == null)
                    return new LoginResponses(new LoginProperties
                    {
                        Flag = true,
                        Token = null!,
                        Role = Roles.None,
                        Message = "Error Occurred! Login container is empty"
                    });

                var getUser = context.ApplicationUser.FirstOrDefault(a => a.EmailId == user.EmailId);
                if (getUser is null)
                    return new LoginResponses(new LoginProperties
                    {
                        Flag = true,
                        Token = null!,
                        Role = Roles.None,
                        Message = "Error Occurred! User not found"
                    });

                var checkPass = context.ApplicationUser.FirstOrDefault(a => a.EmailId == user.EmailId && a.Password == user.Password);

                if (checkPass == null)
                    return new LoginResponses(new LoginProperties
                    {
                        Flag = true,
                        Token = null!,
                        Role =  Roles.None,
                        Message = "Error Occurred! Invalid email/password"
                    });

                var userSession = new UserSession(getUser.Id.ToString(), getUser.FullName, getUser.EmailId, getUser.Role.ToString());
                string token = GenerateToken(userSession);

                return new LoginResponses(new LoginProperties
                {
                    Id = getUser.Id,
                    Flag = false,
                    EmailId = getUser.EmailId!,
                    Token = token!,
                    Role = getUser.Role,
                    FullName = getUser.FullName!,
                    OrganizationId = getUser.OrganizationId!,
                    Message = "Login Completed"
                });
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new LoginResponses(new LoginProperties
                {
                    Flag = true,
                    Token = null!,
                    Role = Roles.None,
                    Message = "Error Occurred!" + ex.Message
                }); ;
            }
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfig:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };
            var token = new JwtSecurityToken(
                issuer: config["JwtConfig:Issuer"],
                audience: config["JwtConfig:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
