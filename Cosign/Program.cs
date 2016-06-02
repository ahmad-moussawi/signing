using Signing;
using System;
using System.IO;
using System.Linq;

namespace Cosign
{
    class Program
    {
        static string version = "0.1.0";

        static string commandName = "cosign";

        static string description = "A tool to sign a file with multiple p7s (detached) signatures, or simply cosigning a file";

        static string usage = "usage: " + commandName + " filepath signature1.p7s [signature2.p7s [signature3.p7s [...]]]";

        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine(commandName + " " + version);
            Console.WriteLine();
            Console.Write(description);

            var toolkit = new Toolkit();
            var file = new Signing.Util.File();

            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments");
                Console.WriteLine(usage);
                return;
            }
            
            var workingDir = Environment.CurrentDirectory;

            // Set the signature and the file name        
            var fileName = args[0];
            var fileContent = file.Read(Path.Combine(workingDir, fileName));
            var signaturesNames = args.ToList().Skip(1);
            var signatures = signaturesNames.Select(x => file.Read(Path.Combine(workingDir, x))).ToList();

            Console.WriteLine($"Reading the file {fileName}, with the following signatures {string.Join(",", signaturesNames)}");


            // Signing the file, you can pass multiple signatures

            var signed = toolkit.Sign(fileContent, signatures);

            file.Write(Path.Combine(workingDir, $"{fileName}.p7m"), signed);
            file.WriteText(Path.Combine(workingDir, $"{fileName}.p7b"), toolkit.ToBase64Format(signed));
        }
    }
}
