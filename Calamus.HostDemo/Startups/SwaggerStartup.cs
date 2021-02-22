using Calamus.HostDemo;
using Calamus.Infrastructure.Extensions;
using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Youle.Sys.WebApi.Startups
{
    public class SwaggerStartup : IHostStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // 添加Swagger有关中间件
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((document, request) =>
                {
                    Func<string, string> tolower = (path) =>
                    {
                        string[] arrs = path.Split('/');
                        return string.Join("/", arrs.Select(item => item.ToFirstCharLower()));
                    };

                    var paths = document.Paths.ToList();
                    document.Paths.Clear();
                    foreach (var path in paths)
                    {
                        document.Paths.TryAdd(tolower(path.Key), path.Value);  // 路径转为小写
                    }
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API文档");
                //c.DefaultModelExpandDepth(-1);  // 不展示模型
                var assembly = GetType().GetTypeInfo().Assembly;
                c.IndexStream = () => assembly.GetManifestResourceStream($"{assembly.GetName().Name}.index.html");  // 用于miniprofiler 监控
            });
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Youle.Sys接口", Version = "v1" });

                /********XML文档注释*******/
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                //var basePath = AppContext.BaseDirectory ;
                //var xmlPath = Path.Combine(basePath, "Calamus.HostDemo.xml");
                //c.IncludeXmlComments(xmlPath, true);

                //xmlPath = Path.Combine(basePath, "Calamus.Models.xml");
                //c.IncludeXmlComments(xmlPath);

                /********JWT 授权请求头*******/
                var security = new OpenApiSecurityRequirement();
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "jwt",
                        Type = ReferenceType.SecurityScheme
                    },
                    Description = "json web token授权请求头，在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                security.Add(scheme, new string[] { });
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("jwt", scheme);

                c.AddFluentValidationRules(); // 支持FluentValidation 验证格式 MicroElements.Swashbuckle.FluentValidation
            });
        }
    }
}
