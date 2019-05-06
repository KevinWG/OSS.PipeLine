using System.Threading.Tasks;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseStandTask<TReq, TRes>
    {
        /// <inheritdoc />
        public BaseStandTask() : this(null)
        {
        }

        public BaseStandTask(TaskMeta meta) : base(meta)
        {
            InstanceType = InstanceType.Stand;
        }
        
        #region 存储处理

        public IStandTaskProvider<TReq> MetaProvider => (IStandTaskProvider<TReq>) m_metaProvider;

        public void RegisteProvider(IStandTaskProvider<TReq> metaPro)
        {
            m_metaProvider = metaPro;
        }

        #endregion


        internal override Task SaveTaskCondition(TaskContext< TReq, TRes> context)
        {
            return SaveTaskCondition(context.req);
        }

  
        protected virtual Task SaveTaskCondition(TaskReq<TReq> reqWithCondition)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<TaskReq<TReq>> GetTaskCondition(string excId)
        {
            return Task.FromResult<TaskReq<TReq>>(null);
        }
    }
}