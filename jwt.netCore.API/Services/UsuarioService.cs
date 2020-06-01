using AutoMapper;
using jwt.netCore.API.Data;
using jwt.netCore.API.Models;
using jwt.netCore.API.Models.DTOs;
using jwt.netCore.API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt.netCore.API.Services
{
	public class UsuarioService : IUsuarioService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public UsuarioService(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}

		/*private static List<Usuarios> usuarios = new List<Usuarios> {
			new Usuarios(),
			new Usuarios { usr_Id = 1, usr_Name = "Sam"}
		};
		*/

		public async Task<ServiceResponse<List<UsuarioDto>>> AddUsuario(AddUsuarioDto novoUsuario)
		{
			var serviceResponse = new ServiceResponse<List<UsuarioDto>>();
			try
			{
				CreatePasswordHash(novoUsuario.usr_Password, out byte[] passwordHash, out byte[] passwordSalt);
				var usuario = _mapper.Map<Usuario>(novoUsuario);
				usuario.usr_PasswordHash = passwordHash;
				usuario.usr_PasswordSalt = passwordSalt;

				await _context.DD_Usuarios.AddAsync(usuario);
				await _context.SaveChangesAsync();
				serviceResponse.Data = (_context.DD_Usuarios.Select(c => _mapper.Map<UsuarioDto>(c))).ToList();
				serviceResponse.Success = true;
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}

		public async Task<ServiceResponse<UsuarioDto>> UpdateUsuario(UsuarioDto updatedUsuario)
		{
			var serviceResponse = new ServiceResponse<UsuarioDto>();
			try
			{
				var usuario = await _context.DD_Usuarios.FirstOrDefaultAsync(c => c.usr_Id == updatedUsuario.usr_Id);
				usuario.usr_Name = updatedUsuario.usr_Name;
				usuario.usr_Login = updatedUsuario.usr_Login;
				if (updatedUsuario.usr_Password != null)
				{
					CreatePasswordHash(updatedUsuario.usr_Password, out byte[] passwordHash, out byte[] passwordSalt);
					usuario.usr_PasswordHash = passwordHash;
					usuario.usr_PasswordSalt = passwordSalt;
				}
				usuario.usr_Email = updatedUsuario.usr_Email;
				usuario.usr_Phone = updatedUsuario.usr_Phone;
				usuario.usr_Modified = updatedUsuario.usr_Modified;
				usuario.usr_ModifiedBy = updatedUsuario.usr_ModifiedBy;
				serviceResponse.Data = _mapper.Map<UsuarioDto>(usuario);
				_context.DD_Usuarios.Update(usuario);
				await _context.SaveChangesAsync();
				serviceResponse.Success = true;
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<UsuarioDto>>> DeleteUsuario(int id)
		{
			var serviceResponse = new ServiceResponse<List<UsuarioDto>>();
			try
			{
				var usuario = _context.DD_Usuarios.First(c => c.usr_Id == id);
				_context.DD_Usuarios.Remove(usuario);
				await _context.SaveChangesAsync();
				serviceResponse.Data = _context.DD_Usuarios.Select(c => _mapper.Map<UsuarioDto>(c)).ToList();
				serviceResponse.Success = true;
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<UsuarioDto>>> GetAllUsuarios()
		{
			var serviceResponse = new ServiceResponse<List<UsuarioDto>>();
			var dbUsuarios = await _context.DD_Usuarios.ToListAsync();
			serviceResponse.Data = dbUsuarios.Select(c => _mapper.Map<UsuarioDto>(c)).ToList();
			serviceResponse.Success = true;
			return serviceResponse;
		}

		public async Task<ServiceResponse<UsuarioDto>> GetUsuarioById(int id)
		{
			var serviceResponse = new ServiceResponse<UsuarioDto>();
			var dbUsuario = await _context.DD_Usuarios.FirstOrDefaultAsync(c => c.usr_Id == id);
			serviceResponse.Data = _mapper.Map<UsuarioDto>(dbUsuario);
			serviceResponse.Success = true;
			return serviceResponse;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
	}
}
