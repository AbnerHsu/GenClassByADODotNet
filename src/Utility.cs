using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenClass
{
    public static class Utility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="namespaceName"></param>
        /// <param name="csName"></param>
        /// <param name="propertyInfo"></param>
        public static void WriteFile(string path,string namespaceName, string csName, Dictionary<string, string> propertyInfo)
        {
            using (var fs = File.Open(string.Format(@"{0}\{1}.cs", path, csName), FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("namespace GenClass");
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
