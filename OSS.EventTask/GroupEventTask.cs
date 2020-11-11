using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventTask.Extension;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class GroupEventTask<TTData, TTRes>
    {
        internal override async Task Processing(TTData data, GroupEventTaskResp<TTRes> res)
        {
            // 获取任务元数据列表
            var tasks = await GetTasks(res.tried_times);
            if (tasks == null || !tasks.Any())
            {
                return;
            }

            // 执行处理结果
            await ExcutingWithTasks(data, res, tasks);
        }


        #region 辅助方法 —— 节点内部任务执行

        private async Task ExcutingWithTasks(TTData data, GroupEventTaskResp<TTRes> nodeResp,
            IList<IEventTask<TTData, TTRes>> tasks)
        {
            GroupExecuteResp<TTData, TTRes> exeResp;
            if (Meta.Process_type == GroupProcessType.Parallel)
                exeResp = await tasks.Executing_Parallel(data);
            else
                exeResp = await tasks.Executing_Serial(data);

            //  处理回退其他任务
            if ((exeResp.status & GroupExecuteStatus.Revert) == GroupExecuteStatus.Revert)
            {
                if (Meta.Process_type == GroupProcessType.Parallel)
                    await exeResp.TaskResults.Executing_ParallelRevert(data);
                else
                    await exeResp.TaskResults.Executing_SerialRevert(data);
            }
        }

        #endregion
    }
}
