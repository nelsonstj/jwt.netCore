using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jwt.netCore.API.Models.DTOs;
using jwt.netCore.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jwt.netCore.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public sealed class UsuarioController : ControllerBase
    {
        //private readonly ILoggedUserService _loggedUserService;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(/*ILoggedUserService loggedUserService,*/ IUsuarioService usuarioService)
        {
            //_loggedUserService = loggedUserService;
            _usuarioService = usuarioService;
        }

        /*[HttpGet]
        public IActionResult Get()
        {
            var user = _loggedUserService.GetLoggedUser<MyLoggedUser>();
            return Ok(user);
        }*/

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _usuarioService.GetAllUsuarios());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _usuarioService.GetUsuarioById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUsuarioDto newUsuario)
        {
            return Ok(await _usuarioService.AddUsuario(newUsuario));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UsuarioDto updatedUsuario)
        {
            var response = await _usuarioService.UpdateUsuario(updatedUsuario);
            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }

        //[HttpPost("{id}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _usuarioService.DeleteUsuario(id);
            if (response.Data == null)
                return NotFound(response);

            return Ok(response);
        }
    }
}