using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Plugs.LogPlug;
using OSS.EventNode.Executor;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseNode<TTData, TTRes>
    {
        #region 节点执行入口

        // 基类入口方法
        public Task<NodeResponse<TTRes>> Process(TTData req)
        {
            var nodeResp = new NodeResponse<TTRes> { node_status = NodeStatus.WaitProcess };
            return TryProcess(req, nodeResp, 0,null);
        }

        /// <summary>
        ///  处理入口
        /// </summary>
        /// <param name="req">请求数据</param>
        /// <param name="triedTimes">已经处理过的次数，重试时需要传入</param>
        /// <param name="taskIds">重试的任务Id</param>
        /// <returns></returns>
        public Task<NodeResponse<TTRes>> Process(TTData req, int triedTimes, params string[] taskIds)
        {
            var nodeResp = new NodeResponse<TTRes> {node_status = NodeStatus.WaitProcess};
            return TryProcess(req, nodeResp, triedTimes, taskIds);
        }

        private async Task<NodeResponse<TTRes>> TryProcess(TTData req,NodeResponse<TTRes> nodeResp,int triedTimes,params string[] taskIds)
        {
            try
            {
                //  检查初始化
                var checkRes = await ProcessCheck(req, nodeResp,triedTimes,taskIds);
                if (!checkRes)
                    return nodeResp;

                // 【2】 任务处理执行方法
                await Excuting(req, nodeResp, triedTimes,taskIds);

                //  【3】 扩展后置执行方法
                await ProcessEnd(req, nodeResp,triedTimes);

                //  结束， 如果节点是暂停状态，需要保存上下文请求信息
                if (nodeResp.node_status == NodeStatus.ProcessPaused)
                    await TrySaveNodeContext(req, nodeResp);
                return nodeResp;
            }
            catch (Exception e)
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = new TTRes().WithResult(SysResultTypes.ApplicationError,
                    "Error occurred during Node [Process]!");

                LogUtil.Error($"sys_ret:{nodeResp.resp.sys_ret}, ret:{nodeResp.resp.ret},msg:{nodeResp.resp.msg}, Detail:{e}",
                    NodeMeta.node_id, ModuleName);
            }

            await TrySaveNodeContext(req, nodeResp);
            return nodeResp;
        }



        #endregion

        #region 生命周期扩展方法

        protected virtual Task<TTRes> ProcessPreCheck(TTData req, int triedTimes)
        {
            return Task.FromResult(new TTRes());
        }

        protected virtual Task ProcessEnd(TTData req, NodeResponse<TTRes> resp, int triedTimes)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法

        private async Task<bool> ProcessCheck(TTData req, NodeResponse<TTRes> nodeResp, int triedTimes,params  string[] taskIds)
        {
            if (triedTimes > 0 && (taskIds == null || taskIds.Length == 0))
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = new TTRes()
                {
                    sys_ret = (int)SysResultTypes.ApplicationError,
                    msg = "Have no tasks to run"
                };
                return false;
            }
            
            var res = await ProcessPreCheck(req, triedTimes);
            if (!res.IsSuccess())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = res;
                return false;
            }
            return true;
        }


        internal virtual async Task Excuting(TTData req, NodeResponse<TTRes> nodeResp, int triedTimes,
            params string[] taskIds)
        {
            // 获取任务元数据列表
            var tasks = await GetTasks();

            if (tasks != null && triedTimes > 0)
                tasks = tasks.Where(t => taskIds.Contains(t.TaskMeta.task_id)).ToList();

            if (tasks == null || !tasks.Any())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = new TTRes().WithResult(SysResultTypes.ApplicationError, ResultTypes.ObjectNull,
                    $"{this.GetType()} have no tasks can be Runed!");
                return;
            }

            if (triedTimes > 0)
                tasks = tasks.Where(t => taskIds.Contains(t.TaskMeta.task_id)).ToList();

            // 执行处理结果
            await ExcutingWithTasks(req, nodeResp, tasks, triedTimes);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task ExcutingWithTasks(TTData req, NodeResponse<TTRes> nodeResp, IList<IBaseTask<TTData>> tasks,
            int triedTimes)
        {
            if (NodeMeta.Process_type == NodeProcessType.Parallel)
                await this.Excuting_Parallel(req, nodeResp, tasks, triedTimes);
            else
                await this.Excuting_Sequence(req, nodeResp, tasks, triedTimes);

            //  处理回退其他任务
            if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
            {
                var revertTasks = triedTimes > 0 ? (await GetTasks()) : tasks;

                if (NodeMeta.Process_type == NodeProcessType.Parallel)
                    await this.Excuting_ParallelRevert(req, nodeResp, revertTasks, nodeResp.block_taskid, triedTimes);
                else
                    await this.Excuting_SequenceRevert(req, nodeResp, revertTasks, nodeResp.block_taskid, triedTimes);
            }
        }
        #endregion
    }
}
