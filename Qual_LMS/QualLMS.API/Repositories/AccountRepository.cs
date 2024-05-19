using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using static QualLMS.Domain.Models.ServiceResponses;

namespace QualLMS.API.Repositories
{
    public class AccountRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccount
    {
        public ResponsesWithData AllUsers(Guid OrganizationId)
        {
            var users = userManager.Users.Where(u => u.OrganizationId == OrganizationId).ToList();
            return new ResponsesWithData(true, users, "Data found!");
        }

        public async Task<GeneralResponses> CreateAccount(UserRegister userModel)
        {
            if (userModel is null) return new GeneralResponses(false, "Model is empty");
            var newUser = new AppUser()
            {
                FullName = userModel.FullName,
                ParentName = userModel.ParentName,
                ParentNumber = userModel.ParentNumber,
                OrganizationId = userModel.OrganizationId,
                Email = userModel.EmailId,
                PhoneNumber = userModel.PhoneNumber,
                PasswordHash = userModel.Password,
                UserName = userModel.EmailId
            };

            if (!Enum.IsDefined(typeof(Roles), userModel.RoleId)) return new GeneralResponses(false, "Invalid Role");

            string roleName = ((Roles)userModel.RoleId).ToString();

            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return new GeneralResponses(false, "User registered already with this Email!");

            var createUser = await userManager.CreateAsync(newUser!, userModel.Password);
            if (!createUser.Succeeded) return new GeneralResponses(false, "Error occured.. please try again");

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("SuperAdmin");
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

        public async Task<LoginResponses> LoginAccount(Login Login)
        {
            LoginProperties properties = new LoginProperties();
            if (Login == null)
                return new LoginResponses(new LoginProperties
                {
                    Flag = false,
                    Token = null!,
                    Role = -1,
                    Message = "Login container is empty"
                });

            var getUser = await userManager.FindByEmailAsync(Login.EmailId);
            if (getUser is null)
                return new LoginResponses(new LoginProperties
                {
                    Flag = false,
                    Token = null!,
                    Role = -1,
                    Message = "User not found"
                });

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, Login.Password);
            if (!checkUserPasswords)
                return new LoginResponses(new LoginProperties
                {
                    Flag = false,
                    Token = null!,
                    Role = -1,
                    Message = "Invalid email/password"
                });

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.NormalizedUserName, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);

            Roles role = (Roles)Enum.Parse(typeof(Roles), getUserRole.FirstOrDefault()!);

            return new LoginResponses(new LoginProperties
            {
                Id = getUser.Id,
                Flag = true,
                EmailId = getUser.Email!,
                Token = token!,
                Role = (int)role,
                FullName = getUser.FullName!,
                OrganizationId = getUser.OrganizationId!,
                ParentName = getUser.ParentName!,
                ParentNumber = getUser.ParentNumber!,
                Message = "Login Completed"
            });
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
