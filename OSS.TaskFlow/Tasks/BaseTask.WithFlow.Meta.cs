using System.Threading.Tasks;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    
    public abstract partial class BaseFlowTask<TReq, TFlowData, TRes> 
    {

        protected BaseFlowTask()
        {
            InstanceType = InstanceType.WithFlow;
        }


        #region 存储处理

        public IFlowTaskMetaProvider<TReq, TFlowData> MetaProvider => (IFlowTaskMetaProvider<TReq, TFlowData>)m_metaProvider;

        public void RegisteProvider(IFlowTaskMetaProvider<TReq, TFlowData> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }


        #endregion


        #region 重写基类方法


        internal override Task SaveTaskContext_Internal(TaskContext context, TaskReqData data)
        {
            return MetaProvider.SaveTaskContext(context, (TaskReqData<TReq, TFlowData>)data);
        }

        #endregion
    }
}