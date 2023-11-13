using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCDashboardAuth.Models;

namespace MVCDashboardAuth.Controllers {
    public class AccountController : Controller {
        // use hard-coded credentials
        private List<Person> people = new List<Person> {
            new Person { Login="admin@gmail.com", Password="12345", Role = "admin" },
            new Person { Login="user@gmail.com", Password="55555", Role = "user" }
        };

        [HttpPost()]
        public ActionResult Token(string username, string password) {
            var identity = GetIdentity(username, password);

            if (identity == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid username or password.");
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password) {
            var person = people.FirstOrDefault(x => x.Login == username && x.Password == password);

            if (person != null) {
                var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}