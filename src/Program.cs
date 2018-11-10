using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.IO;

namespace GenClass
{
    class Program
    {
        static void Main(string[] args)
        {
            IClassFileGenerator generator = new ADODotNet();
            generator.Create();
        }
    }
}
