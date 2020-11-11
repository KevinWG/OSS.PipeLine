using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventNode.Executor;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Extention;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class GroupEventTask<TTData, TTRes>
    {


        internal override  

        private async Task<GroupTaskResp<TTRes>> TryProcess(TTData data, GroupTaskResp<TTRes> nodeResp, int triedTimes,
            params string[] taskIds)
        {
            //  检查初始化
            var checkRes = ProcessCheck(data, nodeResp, triedTimes, taskIds);
            if (!checkRes)
                return nodeResp;

            // 【2】 任务处理执行方法
            await Excuting(data, nodeResp, triedTimes, taskIds);


            //  结束， 如果节点是暂停状态，需要保存上下文请求信息
            if (nodeResp.node_status == NodeStatus.ProcessPaused)
                await TrySaveNodeContext(data, nodeResp);
            return nodeResp;
        }


        #region 生命周期扩展方法


      
        #endregion

        #region 内部扩展方法

        private bool ProcessCheck(TTData data, GroupTaskResp<TTRes> nodeResp, int triedTimes,
            params string[] taskIds)
        {
            if (triedTimes > 0 && (taskIds == null || taskIds.Length == 0))
            {
                nodeResp.run_status =  TaskRunStatus.RunFailed;
                return false;
            }
            return true;
        }


        internal virtual async Task Excuting(TTData data, GroupTaskResp<TTRes> nodeResp, int triedTimes,
            params string[] taskIds)
        {
            // 获取任务元数据列表
            var tasks = await GetTasks();
            if (tasks == null || !tasks.Any())
            {
                return;
            }
            // 执行处理结果
            await ExcutingWithTasks(data, nodeResp, tasks, triedTimes);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task ExcutingWithTasks(TTData data, GroupTaskResp<TTRes> nodeResp, IList<IEventTask<TTData,TTRes>> tasks,
            int triedTimes)
        {
            if (Meta.Process_type == NodeProcessType.Parallel)
                await this.Excuting_Parallel(data, nodeResp, tasks, triedTimes);
            else
                await this.Excuting_Serial(data, nodeResp, tasks, triedTimes);

            //  处理回退其他任务
            if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
            {
                var revertTasks = triedTimes > 0 ? (await GetTasks()) : tasks;

                if (Meta.Process_type == NodeProcessType.Parallel)
                    await this.Excuting_ParallelRevert(data, nodeResp, revertTasks, nodeResp.block_taskid);
                else
                    await this.Excuting_SerialRevert(data, nodeResp, revertTasks, nodeResp.block_taskid);
            }
        }
        #endregion
    }
}
