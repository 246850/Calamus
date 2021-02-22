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
                options.Filters.Add<GlobalExceptionFilterAttribute>();  // ȫ���쳣������
                options.Filters.Add<GenericResultFilterAttribute>();    // ͨ��ִ�н����װ���������
                                                                        //options.Filters.Add<DefaultAuthorizeFilterAttribute>();    // ȫ����Ȩ��֤ʧ���Զ�����������
                //options.UseCentralRoutePrefix(new RouteAttribute("demoapi"));  //ȫ�� ���·�ɿ���ǰ׺
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;  // ������ʵ�� ����ĸ��Сд��
                options.JsonSerializerOptions.WriteIndented = false;
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;// ��ֹĬ��ģ����֤�������
                })
                .AddFluentValidation(config =>  // ����ģ�Ͳ�����֤
                {
                    //config.ValidatorFactory = new FluentValidatorFactory();
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = true;    // false : ��ֹĬ��ģ����֤
                    config.ValidatorOptions.CascadeMode = CascadeMode.Stop; // ��������֤����һ����������ֹͣ
                    //config.RegisterValidatorsFromAssemblyContaining<DepartmentCreateOrUpdateValidator>();
                });
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));// �����������
            services.AddHostedService<NLogHostService>();   // NLog �رշ���
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
