using OSS.Common.ComModels;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseStandTask<TReq, TRes> : BaseTask<ExcuteReq<TReq>, TRes, TReq>, IBaseStandTask<TReq>
        where TRes : ResultMo, new()
    {
        // <inheritdoc />
        protected BaseStandTask() : this(null)
        {
        }

        protected BaseStandTask(TaskMeta meta) : base(meta)
        {
            InstanceTaskType = InstanceType.Stand;
        }
        
        #region 存储处理

        public IStandTaskProvider<TReq> MetaProvider => (IStandTaskProvider<TReq>) m_metaProvider;

        public void RegisteProvider(IStandTaskProvider<TReq> metaPro)
        {
            m_metaProvider = metaPro;
        }

        #endregion

        
    }
}