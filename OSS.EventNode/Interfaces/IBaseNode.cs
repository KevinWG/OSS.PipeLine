using OSS.EventNode.MetaMos;

namespace OSS.EventNode.Interfaces
{
    public interface IBaseNode
    {
        NodeMeta NodeMeta { get; }
    }

    public interface IBaseNode<TTData, TTRes>: IBaseNode
    {
     
    }

}
