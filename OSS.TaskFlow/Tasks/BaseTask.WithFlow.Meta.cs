using System.Threading.Tasks;
using OSS.TaskFlow.Node.MetaMos;
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

        public IFlowTaskProvider<TReq, TFlowData> MetaProvider => (IFlowTaskProvider<TReq, TFlowData>)m_metaProvider;

        public void RegisteProvider(IFlowTaskProvider<TReq, TFlowData> metaPro)
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