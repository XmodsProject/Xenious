using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Xenious.Startup
{
    public class Update
    {
        public static string get_update_info()
        {
            try
            {
                // Get current available info.
                string url = "http://xenious.staticpi.net/downloads/update_info";
                WebRequest myReq = WebRequest.Create(url);
                WebResponse wr = myReq.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd().Replace("\n", "");

                return content;
            }
            catch
            {
                return "Unable to get download information...";
            }
        }
        public static void download_update()
        {
            try
            {
                // Get current available info.
                string url = "https://xenious.staticpi.net/downloads/xenious.zip";
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(url, @AppDomain.CurrentDomain.BaseDirectory + "xenious.zip");
                }

            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        public static void extract_update()
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(AppDomain.CurrentDomain.BaseDirectory + "xenious.zip", AppDomain.CurrentDomain.BaseDirectory);

            // Copy all files.
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Xenious/");

            // Copy each file from main root to main.
            foreach(string file in files)
            {
                File.Copy(file, AppDomain.CurrentDomain.BaseDirectory + Path.GetFileName(file));
            }

            // Delete package directory.
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "Xenious/", true);
        }
        public static bool delete_old()
        {
            try
            {
                // Create backup directory.
                System.IO.Directory.CreateDirectory((AppDomain.CurrentDomain.BaseDirectory + "old/"));

                // Check if old directory already exists.
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                if (System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "old/" + fvi.FileVersion.ToString())) {
                    throw new Exception(string.Format("if you're using a older version dont update o.0"));
                }

                // Create old directory.
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "old/" + fvi.FileVersion.ToString());

                // Get All files in the directory.
                string[] files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);

                // Copy all to old.
                foreach(string file in files)
                {
                    if (Path.GetFileName(file) != "xenious.zip")
                    {
                        File.Move(file, AppDomain.CurrentDomain.BaseDirectory + "old/" + fvi.FileVersion.ToString() + "/" + Path.GetFileName(file));
                    }
                }

                // Remove each file in bin.
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "bin/", true);
            }
            catch
            {
                
            }

            return true;
        }
    }
}
