using System.Collections.Generic;
using System.IO;

namespace GenClass
{
    public static class Utility
    {
        /// <summary>
        /// 寫入檔案
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <param name="namespaceName">Namespace名稱</param>
        /// <param name="csName">class名稱</param>
        /// <param name="propertyInfo">property資訊, Key是欄位名, Value為欄位型別</param>
        public static void WriteFile(string path,string namespaceName, string csName, Dictionary<string, string> propertyInfo)
        {
            using (var fs = File.Open(string.Format(@"{0}{1}{2}.cs", path, ((path[path.Length - 1] != '\\') ? "\\" : ""), csName), FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("namespace " + namespaceName);
                    sw.WriteLine("{");
                    sw.WriteLine("\tpublic class " + csName);
                    sw.WriteLine("\t{");
                    foreach (var field in propertyInfo)
                    {
                        sw.WriteLine("\t\tpublic string {0} {{ get; set; }}", field.Key);
                    }
                    sw.WriteLine("\t}");
                    sw.WriteLine("}");
                }
            }
        }
    }
}
