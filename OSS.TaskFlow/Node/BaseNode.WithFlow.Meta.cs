using OSS.TaskFlow.Node.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlowNode<TReq, TFlowData, TRes>
    {
        protected BaseFlowNode()
        {
            InstanceType = InstanceType.WithFlow;
        }
        

        #region 存储处理

        public IFlowNodeProvider<TReq, TFlowData> MetaProvider => (IFlowNodeProvider<TReq, TFlowData>)m_metaProvider;

        public void RegisteProvider(IFlowNodeProvider<TReq, TFlowData> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }


        #endregion



    }

}
