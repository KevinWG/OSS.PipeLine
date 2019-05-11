using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Plugs.LogPlug;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;
using OSS.EventTask.Util;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// todo  重新激活处理
    /// todo  全部节点回退
    /// todo  保存未激活信息和节点列表
    /// </summary>
    public abstract partial class BaseNode<TTReq, TTRes>
    {
        #region 节点执行入口

        // 重写基类入口方法
        public async Task<NodeResponse<TTRes>> Process(TTReq req)
        {
            var nodeResp = new NodeResponse<TTRes> {node_status = NodeStatus.WaitProcess};

            try
            {
                //  检查初始化
                var checkRes = await ProcessCheck(req, nodeResp);
                if (!checkRes)
                    return nodeResp;

                // 【2】 任务处理执行方法
                await Excuting(req, nodeResp);

                //  【3】 扩展后置执行方法
                await ProcessEnd(req, nodeResp);
                return nodeResp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        #endregion

        #region 生命周期扩展方法

        protected virtual Task<ResultMo> ProcessPreCheck(TTReq req)
        {
            return Task.FromResult(new ResultMo());
        }

        protected virtual Task ProcessEnd(TTReq req, NodeResponse<TTRes> resp)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法


        private async Task<bool> ProcessCheck(TTReq req, NodeResponse<TTRes> nodeResp)
        {
            var checkRes = ProcessCheckInternal(req);
            if (!checkRes.IsSuccess())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = checkRes;
                return false;
            }

            var res = await ProcessPreCheck(req);
            if (!res.IsSuccess())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = checkRes;
                return false;
            }

            return true;
        }

        //  检查context内容
        internal virtual TTRes ProcessCheckInternal(TTReq context)
        {
            if (string.IsNullOrEmpty(NodeMeta?.node_key))
            {
                return new TTRes().WithResult(SysResultTypes.ApplicationError, ResultTypes.InnerError,
                    "node metainfo has error!");
            }


            //if (string.IsNullOrEmpty(context..exc_id))
            //    context.exc_id = DateTime.Now.Ticks.ToString();

            return new TTRes();
        }



        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task Excuting(TTReq req, NodeResponse<TTRes> nodeResp)
        {
            // 获取任务元数据列表
            var tasks = await GetTaskMetas();
            if (tasks == null || !tasks.Any())
                throw new ResultException(SysResultTypes.ApplicationError, ResultTypes.ObjectNull,
                    $"{this.GetType()} have no tasks can be Runed!");

            // 执行处理结果
            await ExcutingWithTasks(req, nodeResp, tasks);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行 —— 分解

        private async Task ExcutingWithTasks(TTReq req, NodeResponse<TTRes> nodeResp, IList<IBaseTask<TTReq>> tasks)
        {
            if (NodeMeta.Process_type == NodeProcessType.Parallel)
            {
                Excuting_Parallel(req, nodeResp, tasks);
            }
            else
            {
                await Excuting_Sequence(req, nodeResp, tasks);
            }
        }

        #region 辅助方法 —— 节点内部任务执行 —— 顺序执行

        ///  顺序执行
        private async Task Excuting_Sequence(TTReq req, NodeResponse<TTRes> nodeResp, IList<IBaseTask<TTReq>> tasks)
        {
            nodeResp.TaskResults = new Dictionary<TaskMeta, TaskResponse<ResultMo>>(tasks.Count);

            var index = 0;
            var haveError = false;
            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 给出最大值，循环内部处理
            for (; index < tasks.Count; index++)
            {
                var tItem = tasks[index];
                var taskResp = await TryGetTaskItemResult(req, tItem, new RunCondition());

                var tMeta = tItem.TaskMeta;
                nodeResp.TaskResults.Add(tMeta, taskResp);

                haveError = FormatNodeErrorResp(nodeResp, taskResp, tMeta);
                if (haveError)
                    break;
            }
            if (haveError)
                await Excuting_SequenceRevert(req, nodeResp, tasks, index);
            else
                nodeResp.node_status = NodeStatus.ProcessCompoleted;
        }
        //  顺序任务的回退处理
        private static async Task Excuting_SequenceRevert(TTReq req, NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTReq>> tasks, int index)
        {
            if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
            {
                for (; index >= 0; --index)
                {
                    var tItem = tasks[index];
                    var rRes = await tItem.Revert(req);
                    if (rRes)
                        nodeResp.TaskResults[tItem.TaskMeta].run_status = TaskRunStatus.RunReverted;
                }
            }
        }


        private static TTRes ConvertToNodeResp(ResultMo taskResp)
        {
            if (taskResp is TTRes nres)
            {
                return nres;
            }

            return taskResp.ConvertToResultInherit<TTRes>();
        }

        #endregion


        ///   并行执行
        private void Excuting_Parallel(TTReq req, NodeResponse<TTRes> nodeResp,
            IList<IBaseTask<TTReq>> tasks)
        {
            var taskResults =
                tasks.ToDictionary(t => t, t => TryGetTaskItemResult(req, t, new RunCondition()));

            try
            {
                var tw = Task.WhenAll(taskResults.Select(tr => tr.Value));
                tw.Wait();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, NodeMeta.node_key, NodeConfigProvider.ModuleName);
            }

            var taskResps = taskResults.ToDictionary(d => d.Key, d => d.Value.Status == TaskStatus.Faulted
                ? new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition())
                : d.Value.Result);

            nodeResp.node_status = NodeStatus.ProcessCompoleted; // 循环里会处理结果，这里给出最大值
            foreach (var tItemRes in taskResps)
            {
                FormatNodeErrorResp(nodeResp, tItemRes.Value, tItemRes.Key.TaskMeta);
            }

            if (nodeResp.node_status == NodeStatus.ProcessFailedRevert)
            {
                //  todo 执行回退
            }
        }

        #endregion

        #region 其他辅助方法

        private static bool FormatNodeErrorResp(NodeResponse<TTRes> nodeResp, TaskResponse<ResultMo> taskResp,
            TaskMeta tMeta)
        {
            var status = NodeStatus.ProcessCompoleted;
            if (!taskResp.run_status.IsCompleted())
            {
                var haveError = true;
                switch (tMeta.node_action)
                {
                    case NodeResultAction.PauseOnFailed:
                        status = NodeStatus.ProcessPaused;
                        break;
                    case NodeResultAction.FailedOnFailed:
                        status = taskResp.run_status == TaskRunStatus.RunFailed
                            ? NodeStatus.ProcessFailed
                            : NodeStatus.ProcessPaused;
                        break;
                    case NodeResultAction.FailedRevrtOnFailed:
                        status = taskResp.run_status == TaskRunStatus.RunFailed
                            ? NodeStatus.ProcessFailedRevert
                            : NodeStatus.ProcessPaused;
                        break;
                    default:
                        haveError = false;
                        break;
                }

                if (haveError)
                {
                    if (status < nodeResp.node_status)
                    {
                        nodeResp.node_status = status;
                        nodeResp.resp = ConvertToNodeResp(taskResp.resp);
                    }

                    return true;
                }
            }

            if (nodeResp.node_status == NodeStatus.ProcessCompoleted && taskResp.resp is TTRes nres)
            {
                nodeResp.resp = nres;
            }

            return false;
        }

        private async Task<TaskResponse<ResultMo>> TryGetTaskItemResult(TTReq req, IBaseTask<TTReq> task,
            RunCondition taskRunCondition)
        {
            if (InstanceNodeType == InstanceType.Stand && task.InstanceTaskType == InstanceType.Domain)
            {
                return new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
                    "StandNode can't use Domain Task!");
            }

            try
            {
                return await task.Run(req, taskRunCondition);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex, NodeMeta.node_key, NodeConfigProvider.ModuleName);
            }

            return new TaskResponse<ResultMo>().WithError(TaskRunStatus.RunFailed, new RunCondition(),
                "Task of node run error!");
        }

        #endregion

    }
}
