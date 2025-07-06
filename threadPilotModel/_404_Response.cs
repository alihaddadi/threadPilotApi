using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace threadPilotModel
{
    public class _404_Response
    {
        public _404_Response()
        {
            err_msg = "";
            err_cod = 0;
        }

        public _404_Response(string err_msg, int err_cod)
        {
            this.err_msg = err_msg;
            this.err_cod = err_cod;
        }

        public string err_msg { get; set; }
        public int err_cod { get; set; }



    }
}
