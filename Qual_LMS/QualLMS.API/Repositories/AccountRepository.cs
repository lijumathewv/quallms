using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.API.Repositories
{
    public class AccountRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, CustomLogger logger) : IUserAccount
    {
        public async Task<ResponsesWithData> GetStudents(string OrgId)
        {
            try
            {
                Roles role = Roles.Students;
                var teacherrole = await roleManager.FindByNameAsync(role.ToString());

                if (teacherrole == null)
                {
                    return new ResponsesWithData(false, "", "Error Occurred! Role not found");
                }

                var users = await userManager.GetUsersInRoleAsync(role.ToString());
                var result = users.Where(u => u.OrganizationId == new Guid(OrgId)).Select(user => new UserAllData
                {
                    Id = new Guid(user.Id),
                    EmailId = user.Email!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = role.ToString()
                }).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", ex.Message);
            }
        }

        public async Task<ResponsesWithData> GetTeachers(string OrgId)
        {
            try
            {
                Roles role = Roles.Teachers;
                var teacherrole = await roleManager.FindByNameAsync(role.ToString());

                if (teacherrole == null)
                {
                    return new ResponsesWithData(false, "", "Error Occurred! Role not found");
                }

                var users = await userManager.GetUsersInRoleAsync(role.ToString());
                var result = users.Where(u => u.OrganizationId == new Guid(OrgId)).Select(user => new UserAllData
                {
                    Id = new Guid(user.Id),
                    EmailId = user.Email!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = role.ToString()
                }).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", ex.Message);
            }
        }

        public async Task<ResponsesWithData> GetUser(string Id)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(u => u.Id == Id);
                var userRoles = await userManager.GetRolesAsync(user);

                UserAllData result = new UserAllData()
                {
                    Id = new Guid(user.Id),
                    EmailId = user.Email!,
                    FullName = user.FullName!,
                    ParentName = user.ParentName!,
                    ParentNumber = user.ParentNumber!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = userRoles.FirstOrDefault()!
                };

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(true, JsonSerializer.Serialize(new UserAllData()), "Error Occurred! " + ex.Message);
            }
        }

        public async Task<ResponsesWithData> AllUsers(Guid OrganizationId)
        {
            try
            {
                var users = userManager.Users.Where(u => u.OrganizationId == OrganizationId).ToList();

                List<UserAllData> result = new List<UserAllData>();
                foreach (var user in users)
                {
                    var userdata = new UserAllData
                    {
                        Id = new Guid(user.Id),
                        EmailId = user.Email!,
                        FullName = user.FullName!,
                        ParentName = user.ParentName!,
                        ParentNumber = user.ParentNumber!,
                        PhoneNumber = user.PhoneNumber!
                    };

                    var userRoles = await userManager.GetRolesAsync(user);

                    userdata.Role = userRoles.FirstOrDefault()!;
                    result.Add(userdata);
                }

                return new ResponsesWithData(true, JsonSerializer.Serialize(result), "Data found!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(true, JsonSerializer.Serialize(new UserAllData()), "Error Occurred! " + ex.Message);
            }
        }

        public async Task<GeneralResponses> ChangePassword(ChangePassword change)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(change.EmailId);
                if (user == null)
                {
                    // Handle user not found
                    return new GeneralResponses(false, "Error Occurred! User not found!");
                }

                // Verify the old password
                var isOldPasswordCorrect = await userManager.CheckPasswordAsync(user, change.CurrentPassword);
                if (!isOldPasswordCorrect)
                {
                    // Handle incorrect old password
                    return new GeneralResponses(false, "Error Occurred! Incorrect old password.");
                }

                // Change the password
                var result = await userManager.ChangePasswordAsync(user, change.CurrentPassword, change.NewPassword);
                if (result.Succeeded)
                {
                    // Password changed successfully
                    return new GeneralResponses(true, "Password Changed Successfully!");
                }
                else
                {
                    return new GeneralResponses(false, "Error changing password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new GeneralResponses(false, "Error Occurred! " + ex.Message);
            }
        }

        public async Task<GeneralResponses> CreateAccount(UserRegister userModel)
        {
            try
            {
                if (userModel is null) return new GeneralResponses(false, "Error Occurred! Model is empty");
                var newUser = new User()
                {
                    FullName = userModel.FullName,
                    OrganizationId = userModel.OrganizationId,
                    Email = userModel.EmailId,
                    PhoneNumber = userModel.PhoneNumber,
                    PasswordHash = userModel.Password,
                    UserName = userModel.EmailId,
                    CreatedAt = DateTime.Now
                };

                if (!Enum.IsDefined(typeof(Roles), userModel.RoleId)) return new GeneralResponses(false, "Error Occurred! Invalid Role");

                string roleName = ((Roles)userModel.RoleId).ToString();

                var user = await userManager.FindByEmailAsync(newUser.Email);
                if (user is not null) return new GeneralResponses(false, "Error Occurred! User registered already with this Email!");

                var createUser = await userManager.CreateAsync(newUser!, userModel.Password);
                if (!createUser.Succeeded)
                {
                    string PasswordMessage = "";
                    foreach (var r in createUser.Errors)
                    {
                        PasswordMessage += r.Description + "\r\n";
                    }
                    return new GeneralResponses(false, "Error occured..\r\n" + PasswordMessage);

                }

                //Assign Default Role : Admin to first registrar; rest is user
                var checkAdmin = await roleManager.FindByNameAsync("SuperAdmin");

                if (checkAdmin is not null)
                {
                    if (checkAdmin!.Name == roleName && roleName == "SuperAdmin")
                    {
                        return new GeneralResponses(false, "Error Occurred! Invalid Role");
                    }
                }

                if (checkAdmin is null)
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                    await userManager.AddToRoleAsync(newUser, "SuperAdmin");

                    return new GeneralResponses(true, "Account Created");
                }
                else
                {
                    var checkUser = await roleManager.FindByNameAsync(roleName);

                    if (checkUser is null)
                        await roleManager.CreateAsync(new IdentityRole() { Name = roleName });

                    await userManager.AddToRoleAsync(newUser, roleName);

                    return new GeneralResponses(true, "Account Created");
                }
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                
                return new GeneralResponses(false, "Error Occurred! " + logger.GenerateDetailedException(ex));
            }
        }

        public async Task<LoginResponses> LoginAccount(Login Login)
        {
            try
            {
                LoginProperties properties = new LoginProperties();
                if (Login == null)
                    return new LoginResponses(new LoginProperties
                    {
                        Flag = true,
                        Token = null!,
                        Role = "None",
                        Message = "Error Occurred! Login container is empty"
                    });

                var getUser = await userManager.FindByEmailAsync(Login.EmailId);
                if (getUser is null)
                    return new LoginResponses(new LoginProperties
                    {
                        Flag = true,
                        Token = null!,
                        Role = "None",
                        Message = "Error Occurred! User not found"
                    });

                bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, Login.Password);
                if (!checkUserPasswords)
                    return new LoginResponses(new LoginProperties
                    {
                        Flag = true,
                        Token = null!,
                        Role = "None",
                        Message = "Error Occurred! Invalid email/password"
                    });

                var getUserRole = await userManager.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id, getUser.NormalizedUserName, getUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);

                Roles role = (Roles)Enum.Parse(typeof(Roles), getUserRole.FirstOrDefault()!);

                return new LoginResponses(new LoginProperties
                {
                    Id = getUser.Id,
                    Flag = false,
                    EmailId = getUser.Email!,
                    Token = token!,
                    Role = getUserRole.FirstOrDefault()!,
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
                    Role = "None",
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
