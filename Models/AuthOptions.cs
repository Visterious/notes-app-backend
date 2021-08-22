using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NotesAPI
{
    public class AuthOptions
    {
        public string Secret { get; set; }

        public int TokenLifetime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}
