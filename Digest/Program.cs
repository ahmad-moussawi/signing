using Signing;
using System;
using System.IO;

namespace Digest
{
    class Program
    {
        static string version = "0.1.0";

        static string commandName = "digest";

        static string description = "Generate a Sha256 Digest for a file";

        static string usage = "usage: " + commandName + " filepath";

        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine(commandName + " " + version);
            Console.WriteLine();
            Console.Write(description);

            var toolkit = new Toolkit();
            var file = new Signing.Util.File();

            if (args.Length < 1)
            {
                Console.WriteLine("Invalid arguments");
                Console.WriteLine(usage);
                return;
            }

            var workingDir = Environment.CurrentDirectory;

            // Set the signature and the file name        
            var fileName = args[0];
            var fileContent = file.Read(Path.Combine(workingDir, fileName));


            var digest = toolkit.Digest(fileContent);

            file.WriteText(Path.Combine(workingDir, $"{fileName}.sha256digest"), digest);
        }
    }
}
