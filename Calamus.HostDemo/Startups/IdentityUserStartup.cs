using Calamus.AspNetCore.Users;
using Calamus.Infrastructure.TextJson;
using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Claims;

namespace Youle.Sys.WebApi.Startups
{
    /// <summary>
    /// 注册 当前授权用户 IIdentityUser
    /// </summary>
    public class IdentityUserStartup : IHostStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            //services.AddIdentityUser<int, AuthUserModel>((user, principal) =>
            //{
            //    user.Name = string.Empty;
            //    user.Account = string.Empty;
            //    user.Id = Convert.ToInt32(principal.Claims.First(x => x.Type == "id").Value);

            //    var userJson = principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            //    AuthUserModel userInfo = userJson.Deserialize<AuthUserModel>();
            //    user.UserInfo = userInfo;
            //});
        }
    }
}
