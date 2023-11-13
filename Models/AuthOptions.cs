using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MVCDashboardAuth.Models {
    public class AuthOptions {
        public const string ISSUER = "MyAuthServer"; // token provider
        public const string AUDIENCE = "MyAuthClient"; // token consumer
        const string KEY = "mysupersecret_secretkey!123_long_string"; // hash key 
        public const int LIFETIME = 20; // token lifetime in minutes
        public static SymmetricSecurityKey GetSymmetricSecurityKey() {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}