using ApiBase.Contract.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using ShopOnline.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopOnline.Api.Repositories
{
    public class LoginRepository : ILoginReposiitory
    {
        private readonly ShopOnlineDbContext dbContext;
        private readonly IConfiguration configuration;

        public LoginRepository(ShopOnlineDbContext shopOnlineDbContext, IConfiguration configuration)
        {
            this.dbContext = shopOnlineDbContext;
            this.configuration = configuration;
        }
        public async Task<(LoginResponse?, string)> LoginAsync(LoginDto loginDto)
        {
            try
            {
             

                LoginResponse loginResponse = new();

                Login login = this.GetLogin(loginDto.Email);

                if (login is null)
                    return (null, ResponseCode.LoginNotFound);

                if (!VerifyPasswordHash(loginDto.Password, login.PasswordHash, login.PasswordSalt))
                    return (null, ResponseCode.PasswordIncorrect);

                if (login.IsActive == false)
                    return (null, ResponseCode.LoginDeactivated);

                User? user = await dbContext.Users.FindAsync(login.UserId);

                // TODO: Get employee object here and create token with meaningfull data.
                // EmployeeInfoDto employee = await employeeService.GetEmployeeAsync(login.EmployeeId);

                // TODO: Uncomment these if applicable.
                //if (employee is null)
                //    return await Task.FromResult(ResponseCode.EmployeeNotFound);
                //else if (!employee.IsActive)
                //    return await Task.FromResult(ResponseCode.EmployeeDeactivated);

                // TODO: Pass here employee object if needed.
                var token = CreateToken(new Login
                {
                    Id = login.Id,
                    Email = login.Email,
                    UserTypeId = login.UserTypeId
                });
                loginResponse.Token = token;

                if (user != null)
                {
                    loginResponse.User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserTypeId = login.UserTypeId
                    };
                }

                return (loginResponse, ResponseCode.LoginSuccess);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UserRegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var IsEmail = await this.dbContext.Users.Where(x => x.Email.Equals(registerDto.Email) == true && x.IsActive == true).FirstOrDefaultAsync();
                if (IsEmail != null)
                {
                    return false;
                }
                var register = new Login
                {
                    Email = registerDto.Email,
                    User = new User
                    {
                        FirstName = registerDto.FirstName,
                        LastName = registerDto.LastName,
                        Email = registerDto.Email,
                        CreatedTime = registerDto.CreatedTime,
                        CreatedBy = registerDto.CreatedBy,
                    },
                    IsActive = registerDto.IsActive,
                    UserTypeId = UserType.User,

                };
                registerDto.Password = "NSSI@123";

                using (var hmac = new HMACSHA512())
                {
                    register.PasswordSalt = hmac.Key;
                    register.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password));
                }

                this.dbContext.Add(register);
                await this.dbContext.SaveChangesAsync();
                registerDto.UserId = register.User.Id;
                registerDto.Id = register.Id;

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       

        #region Helpers

        /// <summary>
        /// Get login by username to match password entered by user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private Login GetLogin(string email)
        {
            return dbContext.Logins.Include(x => x.User).Where(x => x.User.Email == email || x.Email == email).FirstOrDefault();
        }

        /// <summary>
        ///Generate a random string with a given size and case.   
        /// </summary>
        /// <param size="username"></param>
        /// <returns></returns>
        public string GeneratePassword()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < 8; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
                builder.Append(random.Next(0, 9).ToString("D2"));
            }
            return builder.ToString();
        }
        /// <summary>
        /// Accepts password in plaintext and generates hash and salt for the same.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void EncryptPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies whether user has entered valid password by comparing its hash value.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Verifies whether user has entered valid password by comparing its hash value.
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private void ChangePasswordHash(string newPassword, string confirmPassword, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
            }
        }

        /// <summary>
        /// Generates Jwt token.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        private string CreateToken(Login login)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Email", string.IsNullOrEmpty(login.Email) ? "" : login.Email),
                new Claim("Id", login.Id == default(int) ? "" : login.Id.ToString()),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserType),login.UserTypeId) ?? "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection($"BackendConfig:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        #endregion

    }
}
