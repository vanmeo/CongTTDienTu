using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Xim.AppApi.Constants;
using Xim.AppApi.Jwts;
using Xim.Application.Contracts.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Xim.AppApi.Controllers
{
    /// <summary>
    /// API liên quan tới tài khoản người dùng: đăng nhập, xuất...
    /// </summary>

    public class AccountController : BaseController
    {
       
        private readonly IUserService _service;
       
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _service = serviceProvider.GetService<IUserService>();
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="model"> tên đăng nhập, mật khẩu: admin/123456</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserDtoLogin model)
        {
            var user = await _service.GetLoginAsync(model);
            var jwtConfig = _serviceProvider.GetService<JwtConfig>();
            var token = CreateJwtToken(jwtConfig, user.username, user.id);
            return Ok(token);
        }

        //[Authorize]
        //[HttpPatch("password")]
        //public async Task<IActionResult> ChangePasswordAsync([FromBody] UserDtoChangePassword model)
        //{
        //    var contextData = this.GetContext();
        //    model.userid = contextData.UserId;
        //    await _service.ChangePasswordAsync(model);
        //    return Ok();
        //}

        /// <summary>
        /// Buid login token
        /// </summary>
        static JwtTokenInfo CreateJwtToken(JwtConfig jwt, string username, object userId, string language = null, int? loginType = (int)LoginType.Web, int? tokenExpiredSecond = null, bool refreshToken = true)
        {
            DateTime createDate = DateTime.Now;
            var expireSecond = tokenExpiredSecond ?? jwt.TokenExpiredSecond;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(expireSecond);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwt.Secret);

            var tokenId = Guid.NewGuid().ToString();
            var claimns = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(TokenKey.USERID, userId.ToString()),
                new Claim(TokenKey.USERNAME, username ?? ""),
            };

            if (!string.IsNullOrEmpty(language))
            {
                claimns.Add(new Claim(TokenKey.LANGUAGE, language));
            }

            if (loginType != null)
            {
                claimns.Add(new Claim(TokenKey.TYPE, loginType.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimns),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var rs = new JwtTokenInfo
            {
                Token = $"{jwtToken}",
                Created = createDate,
                ExpiredSecond = expireSecond
            };

            if (refreshToken)
            {
                rs.RefreshToken = $"{CreateRefreshToken(jwt, tokenId)}";
            }

            return rs;
        }

        public static string CreateRefreshToken(JwtConfig jwt, string tokenId)
        {
            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(jwt.RefreshExpiredSecond);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwt.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                }),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }

        class JwtTokenInfo
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public DateTime Created { get; set; }
            public int ExpiredSecond { get; set; }
        }
    }
}
