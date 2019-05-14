using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{

    public abstract partial class BaseDomainTask<TDomain, TReq, TRes> : BaseTask<ExecuteData<TDomain, TReq>, TRes, TReq>
        where TRes : ResultMo, new()
    {
        /// <inheritdoc />
        protected BaseDomainTask() : this(null)
        {
        }

        protected BaseDomainTask(TaskMeta meta) : base(meta)
        {
            InstanceTaskType = InstanceType.Domain;
        }

   

        #region 存储处理

        public IDomainTaskProvider<TReq, TDomain> TaskProvider => (IDomainTaskProvider<TReq, TDomain>) m_metaProvider;

        public void RegisteProvider(IDomainTaskProvider<TReq, TDomain> metaPro)
        {
            m_metaProvider = metaPro;
        }

        #endregion

       

    }
}