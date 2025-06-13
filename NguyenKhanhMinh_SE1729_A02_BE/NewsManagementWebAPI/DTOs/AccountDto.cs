using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementWebAPI.DTOs
{
    public class AccountRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AccountResponseDto
    {
        public string Token { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public int? Role { get; set; }
        public string AccountId { get; set; }
    }
}
