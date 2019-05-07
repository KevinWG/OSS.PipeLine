using System.Threading.Tasks;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{

    public abstract partial class BaseDomainTask<TDomain, TReq, TRes>
    {
        /// <inheritdoc />
        public BaseDomainTask() : this(null)
        {
        }

        public BaseDomainTask(TaskMeta meta) : base(meta)
        {
            InstanceType = InstanceType.Domain;
        }

   

        #region 存储处理

        public IDomainTaskProvider<TReq, TDomain> TaskProvider => (IDomainTaskProvider<TReq, TDomain>) m_metaProvider;

        public void RegisteProvider(IDomainTaskProvider<TReq, TDomain> metaPro)
        {
            m_metaProvider = metaPro;
        }

        #endregion

        // 领域数据需要保持独立源，且其状态会受其他并行节点发生变化，这里不会保存
        internal override Task SaveTaskCondition(TaskContext<TDomain, TReq, TRes> context)
        {
            return SaveTaskCondition(context.req,context.task_condition);
        }


        /// <summary>
        ///  保存环境相关信息【仅在 OwnerType = OwnerType.Task 时发生】
        ///     节点下的环境信息，由节点内部处理，重试的处理也有节点自行触发 
        ///     领域数据需要保持独立源，且其状态会受其他并行节点发生变化，这里不会保存
        /// </summary>
        /// <param name="req"></param>
        /// <param name="runCondition"></param>
        /// <returns></returns>
        protected virtual Task SaveTaskCondition(BaseReq<TReq> req,RunCondition runCondition)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  获取上次对应的环境信息
        /// </summary>
        /// <param name="excId"></param>
        /// <returns></returns>
        protected virtual Task<BaseReq<TReq>> GetTaskCondition(string excId)
        {
            return Task.FromResult<BaseReq<TReq>>(null);
        }
    }
}