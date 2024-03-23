using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VetClinicServer.Settings
{
    public class JwtOptions
    {
        const string KEY = "1nonamesecretkey2nonamesecretkey3nonamesecretkey";
        public const string ISSUER = "VetClinicServer";
        public const string AUDIENCE = "VetClinicClient";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
    }
}
