using System.Collections.Generic;
using OSS.EventFlow.NodeWorker.Mos;

namespace OSS.EventFlow.FlowLine
{
    public class FlowLineInfo
    {
        public string name { get; set; }

        public string flow_code { get; set; }

        public List<NodeInfo> nodes { get; set; }
    }
}
