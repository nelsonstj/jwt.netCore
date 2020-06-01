using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt.netCore.API.Models.DTOs
{
	public class UsuarioDto
	{
        public int usr_Id { get; set; }
        public string usr_Login { get; set; }
        public string usr_Email { get; set; }
        public string usr_Password { get; set; }
        public byte[] usr_PasswordHash { get; set; }
        public byte[] usr_PasswordSalt { get; set; }
        public string usr_Name { get; set; }
        public string usr_Phone { get; set; }
        public int? usr_PasswordExpired { get; set; }
        public int? usr_ChangePwNextLogin { get; set; }
        public int? usr_LoginErrorAttempts { get; set; }
        public DateTime? usr_Created { get; set; }
        public int? usr_CreatedBy { get; set; }
        public DateTime? usr_Modified { get; set; }
        public int? usr_ModifiedBy { get; set; }
        public int? usr_Active { get; set; }
    }

	public class AddUsuarioDto
	{
        public string usr_Login { get; set; }
        public string usr_Email { get; set; }
        public string usr_Password { get; set; }
        public string usr_Name { get; set; }
        public string usr_Phone { get; set; }
        public int? usr_PasswordExpired { get; set; }
        public int? usr_ChangePwNextLogin { get; set; }
        public int? usr_LoginErrorAttempts { get; set; }
        public DateTime? usr_Created { get; set; } = DateTime.Now;
        public int? usr_CreatedBy { get; set; }
        public DateTime? usr_Modified { get; set; }
        public int? usr_ModifiedBy { get; set; }
        public int? usr_Active { get; set; } = 1;
    }
}
