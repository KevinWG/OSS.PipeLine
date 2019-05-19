using OSS.EventNode.MetaMos;

namespace OSS.EventNode.Interfaces
{
    public interface IEventNode
    {
        NodeMeta NodeMeta { get; }
    }

    public interface IEventNode<TTData, TTRes>: IEventNode
    {
        
    }

}
