using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.EventNode.Executor;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseNode<TTData, TTRes>
    {
        #region 节点执行入口

        // 基类入口方法
        public Task<NodeResp<TTRes>> Process(TTData data)
        {
            var nodeResp = new NodeResp<TTRes> { node_status = NodeStatus.WaitProcess };
            return TryProcess(data, nodeResp, 0,null);
        }

        /// <summary>
        ///  处理入口
        /// </summary>
        /// <param name="data">请求数据</param>
        /// <param name="triedTimes">已经处理过的次数，重试时需要传入</param>
        /// <param name="taskIds">重试的任务Id</param>
        /// <returns></returns>
        public Task<NodeResp<TTRes>> Process(TTData data, int triedTimes, params string[] taskIds)
        {
            var nodeResp = new NodeResp<TTRes> {node_status = NodeStatus.WaitProcess};
            return TryProcess(data, nodeResp, triedTimes, taskIds);
        }

        private async Task<NodeResp<TTRes>> TryProcess(TTData data,NodeResp<TTRes> nodeResp,int triedTimes,params string[] taskIds)
        {
            //try
            //{
                //  检查初始化
                var checkRes = await ProcessCheck(data, nodeResp,triedTimes,taskIds);
                if (!checkRes)
                    return nodeResp;

                // 【2】 任务处理执行方法
                await Excuting(data, nodeResp, triedTimes,taskIds);

                var pResp = await Processed(data, nodeResp.node_status, nodeResp.TaskResults, triedTimes);
                if (pResp!=null)
                {
                    nodeResp.node_status = pResp.node_status;
                    nodeResp.resp = pResp.resp;
                }

                //  【3】 扩展后置执行方法
                await ProcessEnd(data, nodeResp,triedTimes);

                //  结束， 如果节点是暂停状态，需要保存上下文请求信息
                if (nodeResp.node_status == NodeStatus.ProcessPaused)
                    await TrySaveNodeContext(data, nodeResp);
                return nodeResp;
            //}
            //catch (Exception e)
            //{
            //    nodeResp.node_status = NodeStatus.ProcessFailed;
            //    nodeResp.resp = new TTRes().WithResp(SysRespTypes.ApplicationError,
            //        "Error occurred during Node [Process]!");

            //    LogHelper.Error($"sys_ret:{nodeResp.resp.sys_ret}, ret:{nodeResp.resp.ret},msg:{nodeResp.resp.msg}, Detail:{e}",
            //        NodeMeta.node_id, EventTaskProvider.ModuleName);
            //}

            //await TrySaveNodeContext(data, nodeResp);
            //return nodeResp;
        }

        #endregion

        #region 生命周期扩展方法

        ///// <summary>
        ///// 处理前初始化检查等处理
        ///// </summary>
        ///// <param name="data">处理数据</param>
        ///// <param name="triedTimes">已经处理过的次数</param>
        ///// <returns></returns>
        //protected virtual Task<TTRes> ProcessInitial(TTData data, int triedTimes)
        //{
        //    return Task.FromResult<TTRes>(null);
        //}

        /// <summary>
        ///   关联任务执行后的结果再处理
        /// </summary>
        /// <param name="data">处理数据</param>
        /// <param name="nodeStatus">根据任务结果列表获取的节点状态</param>
        /// <param name="results">任务对应的结果列表</param>
        /// <param name="triedTimes">已经处理过的次数</param>
        /// <returns> 如果返回空，系统返回结果将根据对应任务状态和返回类型取值 </returns>
        protected virtual Task<ProcessResp<TTRes>> Processed(TTData data, NodeStatus nodeStatus, IDictionary<TaskMeta, TaskResp<TTRes>> results, int triedTimes)
        {
            return Task.FromResult<ProcessResp<TTRes>>(null);
        }

        /// <summary>
        /// 节点处理结束
        /// </summary>
        /// <param name="data">处理数据</param>
        /// <param name="resp">节点处理返回值</param>
        /// <param name="triedTimes">已经处理过的次数</param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TTData data, NodeResp<TTRes> resp, int triedTimes)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法

        private async Task<bool> ProcessCheck(TTData data, NodeResp<TTRes> nodeResp, int triedTimes,
            params string[] taskIds)
        {
            if (triedTimes > 0 && (taskIds == null || taskIds.Length == 0))
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                //nodeResp.resp = new TTRes()
                //{
                //    sys_ret = (int)SysRespTypes.ApplicationError,
                //    msg = "Have no tasks to run"
                //};
                return false;
            }
            
            //var res = await ProcessInitial(data, triedTimes);
            //if (res!=null&&!res.IsSuccess())
            //{
            //    nodeResp.node_status = NodeStatus.ProcessFailed;
            //    //nodeResp.resp =new TTRes().WithResp(res);// res.ConvertToResultInherit<TTRes>();
            //    return false;
            //}
            return true;
        }



        internal virtual async Task Excuting(TTData data, NodeResp<TTRes> nodeResp, int triedTimes,
            params string[] taskIds)
        {
            // 获取任务元数据列表
            var tasks = await GetTasks();

            //if (tasks != null && triedTimes > 0)
            //    tasks = tasks.Where(t => 
            //        taskIds.Contains(t.TaskMeta.task_id)
            //        ).ToList();

            if (tasks == null || !tasks.Any())
            {
                //nodeResp.node_status = NodeStatus.ProcessFailed;
                //nodeResp.resp = new TTRes().WithResp(SysRespTypes.ApplicationError, ResultTypes.ObjectNull,
                //    $"{this.GetType()} have no tasks can be Runed!");
                return;
            }

            // 执行处理结果
            await ExcutingWithTasks(data, nodeResp, tasks, triedTimes);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task ExcutingWithTasks(TTData data, NodeResp<TTRes> nodeResp, IList<IEventTask<TTData,TTRes>> tasks,
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
