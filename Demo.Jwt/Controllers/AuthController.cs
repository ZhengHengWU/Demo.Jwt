using Demo.Jwt.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Jwt.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtSetting _jwtSetting;
        public AuthController(IOptions<JwtSetting> option)
        {
            _jwtSetting = option.Value;
        }
        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/GetToken")]
        public IActionResult GetToken(UserEntity user)
        {
            //创建用户身份标识,这里可以随意加入自定义的参数，key可以自己随便起
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.NameIdentifier, user.username.ToString()),
                    new Claim("Id", user.id.ToString()),
                    new Claim("Name", user.username.ToString())
                };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            var token = new JwtSecurityToken(
                //颁发者
                issuer: _jwtSetting.Issuer,
                //接收者
                audience: _jwtSetting.Audience,
                //过期时间
                expires: DateTime.Now.AddMinutes(30),
                //签名证书
                signingCredentials: creds,
                //自定义参数
                claims: claims
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwtToken);
        }
        /// <summary>
        /// login
        /// </summary>
        /// <param name="userName">只能用user或者</param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Get")]
        public IActionResult Get()
        {
            return Ok("Hello");
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Authorize]
        [Route("api/GetData")]
        public IActionResult GetData()
        {
            var userName = this.CurUserName();
            return Ok(userName);
        }
    }
}