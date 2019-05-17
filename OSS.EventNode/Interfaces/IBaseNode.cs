using OSS.EventNode.MetaMos;

namespace OSS.EventNode.Interfaces
{

    public interface IBaseNode<TTData, TTRes>
    {
        NodeMeta NodeMeta { get; }
        //InstanceType InstanceNodeType { get; }
    }

}
