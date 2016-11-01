using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Xenious.Network;

namespace Xenious
{
    static class Program
    {
        public static List<string> imports_available;

        // Network.
        public static WebAPI wapi;

        static bool is_update()
        {
            try
            {
                // Get current available version.
                string url = "http://xenious.staticpi.net/downloads/current";
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
        static void startup_tasks()
        {
            Startup.Tasks.check_kernal_imports();
            Startup.Tasks.start_wapi();
        }
        static void shutdown_tasks()
        {
            Shutdown.Tasks.shutdown_wapi();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check for update.
            if (is_update() == true)
            {
                DialogResult dr = MessageBox.Show("There is a new version available, would you like to download it ?", "New Update...", MessageBoxButtons.YesNo);
                if(dr == DialogResult.Yes)
                {
                    if(Startup.Tasks.do_update() == true)
                    {
                        return;
                    }
                }
            }

            // Do some startup tasks.
            startup_tasks();

            // Run Launcher.
            Application.Run(new Launcher());

            // Do shutdown tasks.
            shutdown_tasks();
        }
    }
}
