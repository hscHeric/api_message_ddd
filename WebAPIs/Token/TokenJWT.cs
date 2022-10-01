using System.IdentityModel.Tokens.Jwt;

namespace WebAPIs.Token
{
    public class TokenJWT
    {
        private JwtSecurityToken Token;

        internal TokenJWT(JwtSecurityToken token)
        {
            Token = token;
        }

        public DateTime ValidTo => Token.ValidTo;

        public string value => new JwtSecurityTokenHandler().WriteToken(Token);
    }
}
