namespace OSS.EventNode.MetaMos
{
    public class NodeMeta
    {

        public string flow_key { get; set; }

        public string node_key { get; set; }

        public string node_name { get; set; }

        public NodeProcessType Process_type { get; set; }
    }


    public enum NodeProcessType
    {
        Sequence,
        Parallel
    }




}
