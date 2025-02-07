using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ITHelpDesk.Models;
using System.Runtime.InteropServices;

namespace ITHelpDesk.Areas
{
    public class TokenController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly ILogger<LoginModel> _logger;

        public TokenController(SignInManager<IdentityUser> signInManager,
         //   ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           // _logger = logger;
        }
        [HttpPost]
        [Route ("api/Token")]
        public async Task<IActionResult> CreateTokenAsync([FromBody] JwtToken model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.UserName);
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName),

                    };
                    var token = new JwtSecurityToken(
                        JWT.Issuer,
                        JWT.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: creds
                        );
                    var results = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    // return Ok(new JwtSecurityTokenHandler().WriteToken(results));
                    return Created("", results);
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
