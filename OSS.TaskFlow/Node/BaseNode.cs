using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    /// 基础工作节点
    /// todo  重新激活处理
    /// todo  协议处理
    /// todo  全部节点回退
    /// </summary>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BaseNode<TRes> : BaseNode
        where TRes : ResultMo, new()
    {
        #region 对外扩展方法

        protected virtual Task ExcuteEnd(TRes nodeRes, Dictionary<TaskMeta, ResultMo> tastItemDirs,
            NodeContext con)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 重写父类扩展方法

        internal override ResultMo ExcuteResult_Internal(NodeContext con, Dictionary<TaskMeta, ResultMo> taskResDirs)
        {
            var tRes = default(TRes);
            foreach (var tItemPair in taskResDirs)
            {
                var tItemRes = tItemPair.Value;
                if (!tItemRes.IsSysOk())
                {
                    tRes = tItemRes.ConvertToResultInherit<TRes>();
                    break;
                }

                if (tItemRes is TRes res)
                    tRes = res;
            }
            if (tRes == null)
            {
                throw new ArgumentNullException(
                    $"can't find a task of return value match node({this.GetType()}) of return value!");
            }
            return tRes;
        }

        internal override Task ExcuteEnd_Internal(ResultMo nodeRes, Dictionary<TaskMeta, ResultMo> tastItemDirs,
            NodeContext con)
        {
            return ExcuteEnd((TRes)nodeRes, tastItemDirs, con);
        }

        #endregion
    }

    /// <summary>
    ///  基础工作节点
    /// </summary>
    public abstract partial class BaseNode
    {
        #region 节点执行

        /// <summary>
        ///   具体执行方法
        /// </summary>
        /// <param name="con"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        internal async Task<ResultMo> Excute(NodeContext con, TaskReqData req)
        {
            //  检查初始化
            var checkRes = con.CheckNodeContext();
            if (!checkRes.IsSysOk())
                return checkRes.ConvertToResultInherit<ResultMo>();
            // 【1】 扩展前置执行方法
            await ExcutePre_Internal(con, req);

            // 【2】 任务处理执行方法
            var taskResults = await Excuting(con, req);
            if (!taskResults.IsSuccess())
                return taskResults.ConvertToResultInherit<ResultMo>();

            var nodeRes = ExcuteResult_Internal(con, taskResults.data); // 任务结果加工处理

            //  【3】 扩展后置执行方法
            await ExcuteEnd_Internal(nodeRes, taskResults.data, con);
            return nodeRes;
        }

        #endregion 

        #region 对外扩展方法

        /// <summary>
        ///  前置进入方法
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task MoveIn(NodeContext con)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部（执行前，执行后）扩展方法

        //  处理结果
        internal abstract ResultMo ExcuteResult_Internal(NodeContext con, Dictionary<TaskMeta, ResultMo> taskResDirs);

        internal abstract Task ExcutePre_Internal(NodeContext con, TaskReqData req);

        internal abstract Task ExcuteEnd_Internal(ResultMo nodeRes, Dictionary<TaskMeta, ResultMo> tastItemDirs,
            NodeContext con);
        #endregion
        
        #region 辅助方法 —— 节点内部任务执行

        private async Task<ResultMo<Dictionary<TaskMeta, ResultMo>>> Excuting(NodeContext con, TaskReqData req)
        {
            // 获取任务元数据列表
            var taskMetaRes = await GetTaskMetas(con);
            if (!taskMetaRes.IsSuccess())
                return taskMetaRes.ConvertToResult<Dictionary<TaskMeta, ResultMo>>();

            var taskMetas = taskMetaRes.data;

            // 获取元信息对应任务处理实例
            var taskDirs = GetTasks(taskMetas);

            // 执行处理结果
            var taskResults = await ExcutingWithTasks(con, req, taskDirs);
            return new ResultMo<Dictionary<TaskMeta, ResultMo>>(taskResults);
        }

        #region 辅助方法 —— 节点内部任务执行 —— 分解


        private async Task<Dictionary<TaskMeta, ResultMo>> ExcutingWithTasks(NodeContext con, TaskReqData req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            Dictionary<TaskMeta, ResultMo> taskResults;

            if (con.node_meta.node_type == NodeType.Parallel)
            {
                taskResults = Excuting_Parallel(con, req, taskDirs);
            }
            else
            {
                taskResults = await Excuting_Sequence(con, req, taskDirs);
            }

            return taskResults;
        }

        /// <summary>
        ///  顺序执行
        /// </summary>
        /// <param name="con"></param>
        /// <param name="req"></param>
        /// <param name="taskDirs"></param>
        /// <returns></returns>
        private static async Task<Dictionary<TaskMeta, ResultMo>> Excuting_Sequence(NodeContext con, TaskReqData req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            var taskResults = new Dictionary<TaskMeta, ResultMo>(taskDirs.Count);
            foreach (var td in taskDirs)
            {
                var context = ConvertToContext(con, td.Key);
                var retRes = await td.Value.ProcessInternal(context, req);
                taskResults.Add(td.Key, retRes);
            }

            return taskResults;
        }

        /// <summary>
        ///   并行执行
        /// </summary>
        /// <param name="con"></param>
        /// <param name="taskDirs"></param>
        /// <returns></returns>
        private static Dictionary<TaskMeta, ResultMo> Excuting_Parallel(NodeContext con, TaskReqData req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            var taskDirRes = taskDirs.ToDictionary(tr => tr.Key, tr =>
            {
                var context = ConvertToContext(con, tr.Key);
                return tr.Value.ProcessInternal(context, req);
            });

            var tAll = Task.WhenAll(taskDirRes.Select(kp => kp.Value));
            try
            {
                tAll.Wait();
            }
            catch
            {
            }

            var taskResults = taskDirRes.ToDictionary(p => p.Key, p =>
            {
                var t = p.Value;
                return t.Status == TaskStatus.Faulted
                    ? new ResultMo(SysResultTypes.InnerError, ResultTypes.InnerError
                        , $"unexcept error with task {p.Key.task_name}({p.Key.task_key})")
                    : t.Result;
            });
            return taskResults;
        }

        private static TaskContext ConvertToContext(NodeContext con, TaskMeta meta)
        {
            var context = con.ConvertToTaskContext();

            //context.body = (TReq) req.body; //  todo 添加协议转化处理
            //context.flow_data = flowData;
            context.task_meta = meta;

            return context;
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行 - 获取对应的task

        private IDictionary<TaskMeta, BaseTask> GetTasks(IList<TaskMeta> metas)
        {
            var taskDirs = new Dictionary<TaskMeta, BaseTask>(metas.Count);
            foreach (var meta in metas)
            {
                var task = GetTaskByMeta(meta);
                if (task == null)
                {
                    throw new ArgumentNullException($"can't find task named {meta.task_name}({meta.task_key})");
                }

                taskDirs.Add(meta, task);
            }

            return taskDirs;
        }

        #endregion

        #endregion
    }
}
