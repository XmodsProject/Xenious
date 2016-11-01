using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Network
{
    public class WebAPI
    {
        public List<SimpleWebServer.WebServer> wbs;

        public void init()
        {
            wbs = new List<SimpleWebServer.WebServer>();
            wbs.Add(new SimpleWebServer.WebServer(send_test_responce, "http://localhost:2674/test/"));
        }

        public static string send_test_responce(HttpListenerRequest request)
        {
            return string.Format("Hello World :)\n" +
                                 "Requested Time : {0}\n" +
                                 "Request : {1}\n", DateTime.Now, request.QueryString);
        }

        public void close()
        {
            if(wbs != null)
            {
                for(int i = 0; i < wbs.Count; i++)
                {
                    wbs[i].Stop();
                }
                wbs = null;
            }
        }
    }
}
