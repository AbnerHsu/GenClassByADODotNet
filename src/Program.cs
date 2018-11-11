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
