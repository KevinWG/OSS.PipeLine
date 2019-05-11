using OSS.EventNode.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode.Interfaces
{

    public interface IBaseNode<TTReq, TTRes>
    {
        NodeMeta NodeMeta { get; }
        InstanceType InstanceNodeType { get; }
    }

}
