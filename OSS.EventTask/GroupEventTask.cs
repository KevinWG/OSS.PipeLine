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
            var tasks = await GetTasks();
            if (tasks == null || !tasks.Any())
            {
                return;
            }

            // 执行处理结果
            await ExcutingWithTasks(data, res, tasks);
        }


        #region 辅助方法 —— 节点内部任务执行

        private async Task ExcutingWithTasks(TTData data, GroupEventTaskResp<TTRes> nodeResp, IList<IEventTask<TTData,TTRes>> tasks)
        {
            GroupExecuteStatus exeStatus;
            if (Meta.Process_type == GroupProcessType.Parallel)
                exeStatus = await this.Executing_Parallel(data, nodeResp, tasks);
            else
                exeStatus= await this.Executing_Serial(data, nodeResp, tasks);

            //  处理回退其他任务
            if ((exeStatus&GroupExecuteStatus.Revert)==GroupExecuteStatus.Revert)
            {
                var revertTasks = tasks;

                if (Meta.Process_type == GroupProcessType.Parallel)
                    await this.Executing_ParallelRevert(data, nodeResp, revertTasks, nodeResp.block_taskid);
                else
                    await this.Executing_SerialRevert(data, nodeResp, revertTasks, nodeResp.block_taskid);
            }
        }
        #endregion
    }
}
