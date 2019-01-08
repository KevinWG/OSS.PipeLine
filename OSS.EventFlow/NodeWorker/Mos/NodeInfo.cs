using System;
using System.Collections.Generic;
using System.Text;

namespace OSS.EventFlow.NodeWorker.Mos
{
  
    public class NodeInfo
    {
        public string NodeName { get; set; }


        public string NodeCode { get; set; }

        /// <summary>
        ///   下个节点代码
        /// </summary>
        public string NextCode { get; set; }
    }
}
