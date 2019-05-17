using System.Threading.Tasks;

namespace OSS.EventNode
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode<TTData, TTRes> 
    {
        public virtual Task MoveIn(object preData)
        {
            return Task.CompletedTask;
        }
    }
}
