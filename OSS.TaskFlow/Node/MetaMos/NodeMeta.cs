namespace OSS.TaskFlow.Node.MetaMos
{
    public class NodeMeta
    {
        public string node_key { get; set; }

        public string node_name { get; set; }

        public NodeExcuteType excute_type { get; set; }
    }


    public enum NodeExcuteType
    {
        Sequence,
        Parallel
    }




}
