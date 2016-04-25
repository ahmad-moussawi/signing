using Signing;
using System;
using System.Collections.Generic;
using System.IO;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var toolkit = new Toolkit();
            var file = new Signing.Util.File();                        

            // Get the current command directory (while development it will be under bin/debug)
            var workingDir = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "sample");
            
            // Set the signature and the file name        
            var signatureFileName = "signature.p7s";
            var fileName = "file.txt";

            Console.WriteLine($"Reading the file {fileName}, with the signature {signatureFileName}");
            var signature = file.Read(Path.Combine(workingDir, signatureFileName));
            var content = file.Read(Path.Combine(workingDir, fileName));

            // Generating a digest
            var digestPath = Path.Combine(workingDir, fileName + ".digest");
            Console.WriteLine($"Generating file Digest at {digestPath}");
            file.WriteText(digestPath, toolkit.Disgest(content));

            // Signing the file, you can pass multiple signatures
            Console.WriteLine("Signing the file ...");
            var signed = toolkit.Sign(new List<byte[]> { signature }, content);

            file.Write(Path.Combine(workingDir, "signed.bin"), signed);
            file.WriteText(Path.Combine(workingDir, "signed.b64"), toolkit.ToBase64Format(signed));

            Console.WriteLine("Done. Press any key to exit");
            Console.ReadLine();
        }
    }
}
