using jwt.netCore.API.Models;
using jwt.netCore.API.Models.DTOs;
using System.Threading.Tasks;

namespace jwt.netCore.API.Services.Interface
{
	public interface IAuthenticationService
	{
		Task<ServiceResponse<string>> Authenticate(LoginUser loginUser);
		Task<ServiceResponse<int>> Register(AddUsuarioDto user, string password);
		Task<bool> UserExists(string username);
	}
}
