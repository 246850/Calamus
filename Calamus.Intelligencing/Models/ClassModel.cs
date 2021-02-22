using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Intelligencing.Models
{
    public class ClassModel
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 当前程序集公开定义的引用类型数量
        /// </summary>
        public int TypeCount { get; set; }
        /// <summary>
        /// 公开类型
        /// </summary>
        public Type[] Types { get; set; }
    }
}
