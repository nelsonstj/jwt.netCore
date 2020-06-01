using AutoMapper;
using jwt.netCore.API.Models;
using jwt.netCore.API.Models.DTOs;

namespace jwt.netCore.API.Data
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Usuario, UsuarioDto>();
			CreateMap<AddUsuarioDto, Usuario>();
			//CreateMap<UpdateUsuarioDto, Usuarios>();
		}
	}
}
