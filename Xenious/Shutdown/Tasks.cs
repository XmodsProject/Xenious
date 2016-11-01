using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Shutdown
{
    public class Tasks
    {
        /*
         * Shutdown Web API
         * Ends any remaining threads on the webapi.
         */
        public static void shutdown_wapi()
        {
            Xenious.Program.wapi.close();
            Xenious.Program.wapi = null;
        }
    }
}
