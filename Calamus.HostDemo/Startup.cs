using Calamus.AspNetCore.Attributes;
using Calamus.HostDemo.Startups;
using Calamus.Infrastructure.TextJson;
using Calamus.Ioc;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Calamus.HostDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilterAttribute>();  // 全局异常过滤器
                options.Filters.Add<GenericResultFilterAttribute>();    // 通用执行结果包装处理过滤器
                                                                        //options.Filters.Add<DefaultAuthorizeFilterAttribute>();    // 全局授权认证失败自定义结果过滤器
                //options.UseCentralRoutePrefix(new RouteAttribute("demoapi"));  //全局 添加路由控制前缀
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;  // 可用于实现 首字母大小写的
                options.JsonSerializerOptions.WriteIndented = false;
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;// 禁止默认模型验证结果处理
                })
                .AddFluentValidation(config =>  // 请求模型参数验证
                {
                    //config.ValidatorFactory = new FluentValidatorFactory();
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = true;    // false : 禁止默认模型验证
                    config.ValidatorOptions.CascadeMode = CascadeMode.Stop; // 不级联验证，第一个规则错误就停止
                    //config.RegisterValidatorsFromAssemblyContaining<DepartmentCreateOrUpdateValidator>();
                });
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));// 解决中文乱码
            services.AddHostedService<NLogHostService>();   // NLog 关闭服务
            services.AddCalamus(Configuration, Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCalamus(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
