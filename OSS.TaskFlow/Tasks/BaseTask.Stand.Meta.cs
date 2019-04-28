using System;
using System.Threading.Tasks;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Tasks.Interfaces;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks
{
    public abstract partial class BaseStandTask<TReq, TRes> 
    {
        protected BaseStandTask()
        {
            InstanceType = InstanceType.Stand;
        }

        #region 存储处理

        public IStandTaskProvider<TReq> MetaProvider => (IStandTaskProvider<TReq>)m_metaProvider;
        
        public void RegisteProvider(IStandTaskProvider<TReq> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }


        #endregion

        
        #region 重写基类方法
        

        internal override Task SaveTaskContext_Internal(TaskContext context, TaskReqData data)
        {
            return MetaProvider.SaveTaskContext(context, (TaskReqData<TReq>)data);
        }

        #endregion
    }
}