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

            // Do some startup tasks.
            startup_tasks();

            // Run Launcher.
            //Application.Run(new Launcher());
            Application.Run(new Xenious.Forms.Editor());

            // Do shutdown tasks.
            shutdown_tasks();
        }
    }
}
