using Calamus.Intelligencing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RazorEngineCore;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calamus.Intelligencing
{
    /// <summary>
    /// 代码生成 中间件
    /// </summary>
    public class CodeIntelligencingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CodeIntelligencingOptions _options;
        /// <summary>
        /// 嵌入资源根路径
        /// </summary>
        private static readonly string _resourcePathBase;
        /// <summary>
        /// 执行程序集
        /// </summary>
        private static readonly Assembly _assembly;
        static CodeIntelligencingMiddleware()
        {
            var type = typeof(CodeIntelligencingMiddleware);
            _resourcePathBase = $"{type.Namespace}.Razors.";
            _assembly = type.Assembly;
        }
        public CodeIntelligencingMiddleware(RequestDelegate next,
            IOptions<CodeIntelligencingOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string html;
            if (Regex.IsMatch(context.Request.Path, ".+?__info__"))
            {
                string assemblyName = context.Request.Query["assembly"];
                string typeName = context.Request.Query["type"];
                if (string.IsNullOrWhiteSpace(assemblyName) || string.IsNullOrWhiteSpace(typeName))
                {
                    html = "<h3>请求参数有误</h3>";
                }
                else
                {
                    Assembly assembly = _options.Assemblies.FirstOrDefault(x => x.GetName().Name == assemblyName.Trim());
                    Type type = assembly?.ExportedTypes.FirstOrDefault(x => x.FullName == typeName);
                    if (assembly == null || type == null)
                    {
                        html = $"<h3>程序集：{assemblyName}中未找到类型：{typeName}</h3>";
                    }
                    else
                    {
                        html = await BuildInfo(assembly, type); // 构建代码信息
                    }
                }
            }
            else
            {
                html = await BuildClass(context.Request.Path);  // 构建类型
            }

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(html);
        }

        /// <summary>
        /// 读取嵌入资源模板内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetResourceTemplate(string fileName)
        {
            using (Stream stream = _assembly.GetManifestResourceStream(_resourcePathBase + fileName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string template = reader.ReadToEnd();
                    return template;
                }
            }
        }

        /// <summary>
        /// 构建配置程序集中所有类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        async Task<string> BuildClass(string path)
        {
            if (_options.Assemblies == null || !_options.Assemblies.Any()) return "未配置程序集";

            IRazorEngine razorEngine = new RazorEngine();
            string template = GetResourceTemplate("class.cshtml");
            IRazorEngineCompiledTemplate<RazorEngineTemplateBase<List<ClassModel>>> compile = await razorEngine.CompileAsync<RazorEngineTemplateBase<List<ClassModel>>>(template);
            string html = compile.Run(modelBuilder =>
            {
                List<ClassModel> models = new List<ClassModel>();
                foreach (var assembly in _options.Assemblies)
                {
                    var types = assembly.ExportedTypes.Where(x => !x.IsValueType);   // 公开类型，且非值类型

                    models.Add(new ClassModel
                    {
                        Path = path,
                        AssemblyName = assembly.GetName().Name,
                        TypeCount = types.Count(),
                        Types = types.ToArray()
                    });
                }

                modelBuilder.Model = models;
            });

            return html;
        }

        /// <summary>
        /// 构建代码信息
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        async Task<string> BuildInfo(Assembly assembly, Type type)
        {
            IRazorEngine razorEngine = new RazorEngine();
            string template = GetResourceTemplate("info.cshtml");
            IRazorEngineCompiledTemplate<RazorEngineTemplateBase<CodeInfoModel>> compile = await razorEngine.CompileAsync<RazorEngineTemplateBase<CodeInfoModel>>(template);
            string html = compile.Run(modelBuilder =>
            {
                CodeInfoModel model = new CodeInfoModel();
                PropertyInfo[] properties = type.GetProperties().Where(x => x.CanWrite).ToArray(); // 当前类型公开定义的可写属性数组

                model.AssemblyName = assembly.GetName().Name;
                model.ClassName = type.Name;
                model.Namespace = type.Namespace;
                model.PropertyNames = properties.Select(x=> x.Name).ToArray();
                try
                {
                    model.Json = JsonSerializer.Serialize(Activator.CreateInstance(type));
                }
                catch
                {
                    // ignore
                    /*****忽略json序列化失败*****/
                }

                modelBuilder.Model = model;
            });

            return html;
        }
    }
}
