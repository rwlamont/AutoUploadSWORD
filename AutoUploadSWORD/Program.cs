using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutoUploadSWORD
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> failed = new List<string>();
            string strCmdText, server, username, password;
            string[] files = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), "*.zip");
            for (int j = 0; j < files.Count(); j++)
            {
                try
                {
                    Console.WriteLine("Please enter server:");
                    server = Console.ReadLine();
                    Console.WriteLine("Please enter username:");
                    username = Console.ReadLine();
                    Console.WriteLine("Please enter password:");
                    password = Console.ReadLine();
                    Console.WriteLine("Started processing " + files[j]);
                    Process p = new Process();
                    strCmdText = "/C curl  -i --data-binary \"@" + files[j] + "\" -H \"Content-Disposition: filename=myDSpaceMETSItem.zip\" -H \"Content-Type: application/zip\" -H \"X-Packaging: http://purl.org/net/sword-types/METSDSpaceSIP\" -H \"X-No-Op: false\" -H \"X-Verbose: true\" --user \"" +username + ":" + password + "\"  " + server;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = strCmdText;

                    p.Start();
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    Console.WriteLine("Finished processing " + files[j]);
                    if (output.Contains("Accepted") != true)
                    {
                        failed.Add(files[j]);
                    }
                    
                    Console.WriteLine();
                    System.Threading.Thread.Sleep(55000);
                }
                catch{
                    failed.Add(files[j]);
                }

            }
            var writer = new StreamWriter("completedFiles.txt");
            foreach (string s in failed)
            {
                writer.WriteLine(s);

            }

            writer.Close();


            Console.WriteLine("All files finished processing ");
            Console.WriteLine("Read a total of " + files.Count() + " files");
            Console.ReadLine();


        }
    }
}
