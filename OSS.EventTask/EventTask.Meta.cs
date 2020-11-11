using System.Threading.Tasks;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class EventTask<TTData, TTRes> 
        : BaseEventTask<EventTaskMeta, TTData, EventTaskResp<TTRes>,TTRes>//, IEventTask<TTData, TTRes>
        where TTData : class
        where TTRes : class, new()
    {

        protected EventTask()
        {
        }

        protected EventTask(EventTaskMeta meta) : base(meta)
        {
        }

        #region 扩展方法

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        ///    【仅在 OwnerType = OwnerType.Task 时发生】
        ///     节点下的环境信息，由节点内部处理,防止节点其他耗时任务造成执行过程中发起重试操作 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskResp"></param>
        /// <returns></returns>
        protected virtual Task SaveTaskContext(TTData data, EventTaskResp<TTRes> taskResp)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}