using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenClass
{
    /// <summary>
    /// 類別檔案生成器介面
    /// </summary>
    public interface IClassFileGenerator
    {
        /// <summary>
        /// 產生
        /// </summary>
        void Create();
    }
}
