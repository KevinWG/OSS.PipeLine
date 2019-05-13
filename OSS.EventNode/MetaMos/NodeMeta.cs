using OSS.EventTask.Mos;

namespace OSS.EventNode.MetaMos
{
    public class NodeMeta
    {

        public string flow_id { get; set; }

        public string node_id { get; set; }

        public string node_alias { get; set; }



        public NodeProcessType Process_type { get; set; }

        public OwnerType owner_type { get; set; }
    }


    public enum NodeProcessType
    {
        Sequence,
        Parallel
    }




}
