using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Calamus.Intelligencing
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 默认分支路由 /codeIntelligencing
        /// </summary>
        /// <param name="app"></param>
        /// <param name="assemblies">需要生成代码的程序集</param>
        public static void UseCodeIntelligencing(this IApplicationBuilder app, params Assembly[] assemblies)
        {
            app.UseCodeIntelligencing(options => options.Assemblies.AddRange(assemblies));
        }
        /// <summary>
        /// 默认分支路由 /codeIntelligencing
        /// </summary>
        /// <param name="app"></param>
        /// <param name="setup">配置项</param>
        public static void UseCodeIntelligencing(this IApplicationBuilder app, [NotNull] Action<CodeIntelligencingOptions> setup)
        {
            app.UseCodeIntelligencing("/codeIntelligencing", setup);
        }

        /// <summary>
        /// 自定义分支路由
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path">分支路由</param>
        /// <param name="setup">配置项</param>
        public static void UseCodeIntelligencing(this IApplicationBuilder app, [NotNull] PathString path, [NotNull] Action<CodeIntelligencingOptions> setup)
        {
            CodeIntelligencingOptions options = new CodeIntelligencingOptions();
            setup(options);

            Func<HttpContext, bool> predicate = context =>
            {
                return context.Request.Path.StartsWithSegments(path, out var remaining);
            };
            app.MapWhen(predicate, b => b.UseMiddleware<CodeIntelligencingMiddleware>(Options.Create(options)));
        }
    }
}
