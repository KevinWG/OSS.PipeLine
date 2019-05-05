using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// todo  重新激活处理
    /// todo  全部节点回退
    /// </summary>
    public abstract partial class BaseNode<TTContext, TTRes>
        where TTContext : NodeContext
        where TTRes : ResultMo, new()
    {
        #region 节点执行入口

        // 重写基类入口方法
        public async Task<TTRes> Excute(TTContext context)
        {
            //  检查初始化
            var res =  ExcuteCheck(context);
            if (!res.IsSuccess())
                return res.ConvertToResultInherit<TTRes>();

            // 【1】 扩展前置执行方法
            await ExcutePre(context);

            // 【2】 任务处理执行方法
            var taskResults = await Excuting(context);
            var nodeRes = GetNodeResult(context, taskResults); // 任务结果加工处理

            //  【3】 扩展后置执行方法
            await ExcuteEnd(context, nodeRes, taskResults);
            return nodeRes;
        }

        #endregion

        #region 生命周期扩展方法

        protected virtual Task ExcutePre(TTContext con)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法重写

        internal abstract Task<TTRes> GetTaskItemResult(TTContext con, IBaseTask task, TaskMeta taskMeta,
            RunCondition taskRunCondition);

        //  检查context内容
        internal virtual ResultMo ExcuteCheck(TTContext context)
        {
            //  todo  状态有效判断等
            if (string.IsNullOrEmpty(context.node_meta?.node_key))
            {
                return new ResultMo(SysResultTypes.ConfigError, ResultTypes.InnerError,
                    "node metainfo has error!");
            }

            if (string.IsNullOrEmpty(context.run_id))
                context.run_id = DateTime.Now.Ticks.ToString();

            return new ResultMo();
        }

        #endregion

        #region 节点生命周期事件扩展方法

        protected virtual Task ExcuteEnd(TTContext con,
            ResultMo nodeRes, Dictionary<TaskMeta, ResultMo> taskResults)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task<Dictionary<TaskMeta, ResultMo>> Excuting(TTContext con)
        {
            // 获取任务元数据列表
            var taskDirs = await GetTaskMetas(con);
            if (taskDirs == null || taskDirs.Count == 0)
                throw new ResultException(SysResultTypes.ConfigError, ResultTypes.ObjectNull,
                    $"{this.GetType()} have no tasks can be processed!");

            // 执行处理结果
            var taskResults = await ExcutingWithTasks(con, taskDirs);
            return new Dictionary<TaskMeta, ResultMo>(taskResults);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行 —— 分解


        private async Task<Dictionary<TaskMeta, ResultMo>> ExcutingWithTasks(TTContext con,
            IDictionary<TaskMeta, IBaseTask> taskDirs)
        {
            Dictionary<TaskMeta, ResultMo> taskResults;

            if (con.node_meta.excute_type == NodeExcuteType.Parallel)
            {
                taskResults = Excuting_Parallel(con, taskDirs);
            }
            else
            {
                taskResults = await Excuting_Sequence(con, taskDirs);
            }

            return taskResults;
        }

        /// <summary>
        ///  顺序执行
        /// </summary>
        /// <param name="con"></param>
        /// <param name="taskDirs"></param>
        /// <returns></returns>
        private async Task<Dictionary<TaskMeta, ResultMo>> Excuting_Sequence(TTContext con,
            IDictionary<TaskMeta, IBaseTask> taskDirs)
        {
            var taskResults = new Dictionary<TaskMeta, ResultMo>(taskDirs.Count);
            foreach (var td in taskDirs)
            {
                var retRes = await GetTaskItemResult(con, td.Value, td.Key, new RunCondition());
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
        private Dictionary<TaskMeta, ResultMo> Excuting_Parallel(TTContext con,
            IDictionary<TaskMeta, IBaseTask> taskDirs)
        {
            var taskDirRes = taskDirs.ToDictionary(tr => tr.Key, tr =>
            {
                var task = tr.Value;
                return GetTaskItemResult(con, tr.Value, tr.Key, new RunCondition());
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


        #endregion

        #region 其他辅助方法

        // 处理结果转换
        internal TTRes GetNodeResult(TTContext con, Dictionary<TaskMeta, ResultMo> taskResDirs)
        {
            var tRes = default(TTRes);
            foreach (var tItemPair in taskResDirs)
            {
                var tItemRes = tItemPair.Value;
                if (!tItemRes.IsSysOk())
                {
                    tRes = tItemRes.ConvertToResultInherit<TTRes>();
                    break;
                }

                if (tItemRes is TTRes res)
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
