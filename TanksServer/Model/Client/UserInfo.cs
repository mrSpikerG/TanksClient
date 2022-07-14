using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksServer.Model
{
    public class UserInfo
    {
        public UserInfo(string login, string password)
        {
            this.Login = login;
            this.Password = string.IsNullOrEmpty(password) ? string.Empty : 
                BitConverter.ToString(new System.Security.Cryptography.SHA256Managed()
                .ComputeHash(System.Text.Encoding.UTF8
                .GetBytes(password)))
                .Replace("-", string.Empty);
        }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
