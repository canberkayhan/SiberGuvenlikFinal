using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CanberkAyhan
{
    class Program
    {
        class NMAP : IDisposable
        {
            private ProcessStartInfo i = new ProcessStartInfo();
            private Process j = new Process();
            private string output;
            private string script;
            public NMAP(string script)
            {
                this.script = script;
                i.Arguments = "-p 80 --script " + this.script + " testphp.vulnweb.com -oX -";
                i.RedirectStandardOutput = true;
                i.FileName = "nmap";
                j.StartInfo = i;
            }
            public void Dispose()
            {
                j.Dispose();
            }
            private void TakeOutput()
            {
                if (string.IsNullOrEmpty(output))
                {
                    j.Start();
                    output = j.StandardOutput.ReadToEnd();
                    j.WaitForExit();
                    j.Close();
                }
            }
            public string StdOut
            {
                get
                {
                    TakeOutput();
                    return output;
                }
            }
        }
        static void Main(String[] args)
        {
            List<NMAP> nmaps = new List<NMAP>();
            nmaps.Add(new NMAP("http-sitemap-generator"));
            nmaps.Add(new NMAP("http-php-version"));
            nmaps.Add(new NMAP("http-sql-injection"));
            nmaps.Add(new NMAP("ssl-ccs-injection"));
            nmaps.Add(new NMAP("http-csrf"));
            StreamWriter sw = new StreamWriter("Çikti.xml");
            nmaps.ForEach(x => sw.WriteLine(x.StdOut));
            sw.Flush();
            sw.Close();
        }
    }
}
