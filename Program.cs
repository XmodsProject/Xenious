using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Xenious
{
    static class Program
    {
        static bool is_update()
        {
            try
            {
                // Get current available version.
                string url = "https://xenious.staticpi.net/downloads/current";
                WebRequest myReq = WebRequest.Create(url);
                WebResponse wr = myReq.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd().Replace("\n", "");
                string[] version = content.Split('.');

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string[] actual_version = fvi.FileVersion.Split('.');

                // Compare versions to see which one is greater.
                if((Int32.Parse(version[0])) > Int32.Parse(actual_version[0]) || // Major
                   (Int32.Parse(version[1])) > Int32.Parse(actual_version[1]) || // Minor
                   (Int32.Parse(version[2])) > Int32.Parse(actual_version[2]) || // Build
                   (Int32.Parse(version[3])) > Int32.Parse(actual_version[3])) {  // QFE
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(is_update() == true)
            {
                DialogResult dr = MessageBox.Show("There is a new version available, would you like to download it ?", "New Update...", MessageBoxButtons.YesNo);
                if(dr == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://xenious.staticpi.net/downloads/xenious.zip");
                    return;
                }
            }

            // Check kernal Folder Exist, creat if not.
            if(Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/kernal/") == false)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/kernal/");
            }
            Application.Run(new Launcher());
        }
    }
}
