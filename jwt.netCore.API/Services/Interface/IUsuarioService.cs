using jwt.netCore.API.Models;
using jwt.netCore.API.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jwt.netCore.API.Services.Interface
{
	public interface IUsuarioService
	{
		Task<ServiceResponse<List<UsuarioDto>>> GetAllUsuarios();
		Task<ServiceResponse<UsuarioDto>> GetUsuarioById(int id);
		Task<ServiceResponse<List<UsuarioDto>>> AddUsuario(AddUsuarioDto usuario);
		Task<ServiceResponse<UsuarioDto>> UpdateUsuario(UsuarioDto usuario);
		Task<ServiceResponse<List<UsuarioDto>>> DeleteUsuario(int id);
	}
}
