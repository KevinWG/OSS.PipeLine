using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
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
    //public abstract class BaseNode<TRes> : BaseNode
    //    where TRes : ResultMo, new()
    //{
    //    #region 对外扩展方法

    //    protected virtual Task ExcuteEnd(TRes nodeRes, Dictionary<TaskMeta, ResultMo> tastItemDirs,
    //        NodeContext con)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    #endregion

    //    #region 重写父类扩展方法

    //    internal override async Task<ResultMo> Excute_Internal(NodeContext con, TaskReqData req)
    //    {
    //        var res = await base.Excute_Internal(con, req);
    //        if (res.IsSuccess())
    //            return res;

    //        if (res is TRes)
    //            return res;

    //        return res.ConvertToResultInherit<TRes>();
    //    }

    //    internal override Task ExcuteEnd_Internal(ResultMo nodeRes, Dictionary<TaskMeta, ResultMo> tastItemDirs,
    //        NodeContext con)
    //    {
    //        return ExcuteEnd((TRes) nodeRes, tastItemDirs, con);
    //    }

    //    #endregion
    //}

    /// <summary>
    ///  基础工作节点
    /// todo  重新激活处理
    /// todo  协议处理
    /// todo  全部节点回退
    /// </summary>
    public abstract partial class BaseNode
    {
        #region 节点执行入口

        /// <summary>
        ///   主入口方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        internal virtual async Task<ResultMo> Excute_Internal(NodeContext context, TaskReqData req)
        {
            //  检查初始化
            CheckInitailNodeContext(context);

            var taskResults = await Excuting(context, req);
            var nodeRes = ExcuteResult_Internal(context, taskResults); // 任务结果加工处理

            //  【3】 扩展后置执行方法
            await ExcuteEnd(context, nodeRes, taskResults);
            return nodeRes;
        }


        #endregion

        #region 节点生命周期事件扩展方法

        protected virtual Task ExcuteEnd(NodeContext con,
            ResultMo nodeRes, Dictionary<TaskMeta, ResultMo> taskResults)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 节点生命周期内部扩展方法

        internal abstract ResultMo ExcuteResult_Internal(NodeContext context,
            Dictionary<TaskMeta, ResultMo> taskResults);

        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task<Dictionary<TaskMeta, ResultMo>> Excuting(NodeContext con, TaskReqData req)
        {
            // 获取任务元数据列表
            var taskDirs = await GetTaskMetas(con);
            if (taskDirs == null || taskDirs.Count == 0)
                throw new ResultException(SysResultTypes.ConfigError, ResultTypes.ObjectNull,
                    $"{this.GetType()} have no tasks can be processed!");

            // 执行处理结果
            var taskResults = await ExcutingWithTasks(con, req, taskDirs);
            return new Dictionary<TaskMeta, ResultMo>(taskResults);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行 —— 分解


        private static async Task<Dictionary<TaskMeta, ResultMo>> ExcutingWithTasks(NodeContext con, TaskReqData req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            Dictionary<TaskMeta, ResultMo> taskResults;

            if (con.node_meta.excute_type == NodeExcuteType.Parallel)
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
                var retRes = await td.Value.Process_Internal(context, req);
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
                return tr.Value.Process_Internal(context, req);
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
            //context.domain = flowData;
            context.task_meta = meta;

            return context;
        }

        #endregion

        #region 其他辅助方法

        //  检查context内容
        private static void CheckInitailNodeContext(NodeContext context)
        {
            //  todo  状态有效判断等
            if (string.IsNullOrEmpty(context.node_meta?.node_key))
            {
                throw new ResultException(SysResultTypes.ConfigError, ResultTypes.InnerError,
                    "node metainfo has error!");
            }

            if (string.IsNullOrEmpty(context.run_id))
                context.run_id = DateTime.Now.Ticks.ToString();
        }

        // 处理结果转换
        internal TRes GetNodeResult<TRes>(NodeContext con, Dictionary<TaskMeta, ResultMo> taskResDirs)
            where TRes : ResultMo, new()
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

        #endregion


    }


}
