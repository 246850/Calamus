using Calamus.Ioc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Youle.Sys.WebApi.Startups
{
    /// <summary>
    /// jwt 认证
    /// </summary>
    public class JwtAuthStartup : IHostStartup
    {
        public int Order => -1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();
#if DEBUG
            IdentityModelEventSource.ShowPII = true;
#endif
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                SecurityKey securityKey;
                if (calamusOptions.Jwt.SecurityType == 1)
                {
                    RSA rsa = RSA.Create();
                    rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(calamusOptions.Jwt.RsaPublicKey), out int reads);  // pkcs 8
                    //rsa.ImportRSAPublicKey(Convert.FromBase64String(calamusOptions.Jwt.RsaPublicKey), out int reads);   // pkcs 1
                    securityKey = new RsaSecurityKey(rsa);
                }
                else
                {
                    securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(calamusOptions.Jwt.SymmetricSecurityKey));
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = calamusOptions.Jwt.ValidateIssuer,
                    ValidIssuer = calamusOptions.Jwt.ValidIssuer,
                    ValidateAudience = calamusOptions.Jwt.ValidateAudience,
                    ValidAudience = calamusOptions.Jwt.ValidAudience,
                    ValidateLifetime = calamusOptions.Jwt.ValidateLifetime, //是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(calamusOptions.Jwt.ClockSkew), // 允许时间偏差,
                    ValidateIssuerSigningKey = calamusOptions.Jwt.ValidateIssuerSigningKey,
                    IssuerSigningKey = securityKey
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        //var jtiClaims = context.Principal.Claims.FirstOrDefault(item => item.Type == JwtRegisteredClaimNames.Jti);

                        //if (cache.TryGetValue(jtiClaims.Value, out object value))   // 缓存中存在，说明token 已注销，已失效
                        //{
                        //    context.Fail("授权已失效");
                        //}
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options => { 
                
            });
        }
    }
}
