using ShopOnline.Models.Dtos;
namespace ShopOnline.Api.Repositories.Contracts
{
    public interface ILoginReposiitory
    {

        /// <summary>
        /// Login with username and password.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        Task<(LoginResponse?, string)> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// User Register with username and password.
        /// </summary>
        /// <returns></returns>
        Task<bool> UserRegisterAsync(RegisterDto registerDto);

    }
}
