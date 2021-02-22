using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calamus.Intelligencing.Models
{
    public class CodeInfoModel
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 当前类型公开定义的可写属性名
        /// </summary>
        public string[] PropertyNames { get; set; }
        public string PropertiesJson
        {
            get
            {
                return JsonSerializer.Serialize(PropertyNames);
            }
        }
        /// <summary>
        /// 当前类型json序列化字符串
        /// </summary>
        public string Json { get; set; }
    }
}
