using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.Intelligencing
{
    public class CodeIntelligencingOptions
    {
        public CodeIntelligencingOptions()
        {
            Assemblies = new List<Assembly>();
        }
        /// <summary>
        /// 需要生成代码的 程序集
        /// </summary>
        public List<Assembly> Assemblies { get; set; }
    }
}
