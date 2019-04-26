using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  基础工作节点
    /// todo  重新激活处理
    /// todo  协议处理
    /// todo   flowentitytype
    /// </summary>
    public abstract partial class BaseNode<TReq> : BaseNode // : INode<TReq>,IFlowNode
    {
        internal override Task<ResultMo> Excute(ExcuteReq fReq, object flowData)
        {
            // 初始化相关上下文信息
            return Excute(fReq);
        }

        protected virtual Task<ResultMo> Excuted(Dictionary<TaskMeta, ResultMo> taskResults)
        {
            var res = taskResults.FirstOrDefault(p => p.Value.sys_ret != 0).Value;
            return Task.FromResult(res ?? new ResultMo());
        }

        #region 节点具体执行

        public async Task<ResultMo> Excute(ExcuteReq req)
        {
            // 获取任务元数据列表
            var taskMetaRes = await GetTaskMetas(req);
            if (!taskMetaRes.IsSuccess())
                return new ResultMo(ResultTypes.UnKnowOperate, "未知的处理操作！");
            var taskMetas = taskMetaRes.data;

            // 获取元信息对应任务处理实例
            var taskDirs = GetTasks(taskMetas);

            // 执行处理结果
            var taskResults = await Excute_Results(req, taskDirs);

            // 处理结束后加工处理
            return await Excuted(taskResults);
        }

        private async Task<Dictionary<TaskMeta, ResultMo>> Excute_Results(ExcuteReq req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            Dictionary<TaskMeta, ResultMo> taskResults;

            if (NodeMeta.node_type == NodeType.Parallel)
            {
                taskResults = Excute_Parallel(req, taskDirs);
            }
            else
            {
                taskResults = await Excute_Sequence(req, taskDirs);
            }

            return taskResults;
        }

        /// <summary>
        ///  顺序执行
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskDirs"></param>
        /// <returns></returns>
        private static async Task<Dictionary<TaskMeta, ResultMo>> Excute_Sequence(ExcuteReq req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            var taskResults = new Dictionary<TaskMeta, ResultMo>(taskDirs.Count);
            foreach (var td in taskDirs)
            {
                var context = ConvertToContext(req, td.Key);
                var retRes = await td.Value.Process(context);
                taskResults.Add(td.Key, retRes);
            }

            return taskResults;
        }

        /// <summary>
        ///   并行执行
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskDirs"></param>
        /// <returns></returns>
        private static Dictionary<TaskMeta, ResultMo> Excute_Parallel(ExcuteReq req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            var taskDirRes = taskDirs.ToDictionary(tr => tr.Key, tr =>
            {
                var context = ConvertToContext(req, tr.Key);
                return tr.Value.Process(context);
            });

            var tAll = Task.WhenAll(taskDirRes.Select(kp => kp.Value));
            try
            {
                tAll.Wait();
            }
            catch
            {
            }

            //if (t.Status == TaskStatus.RanToCompletion)
            //else if (t.Status == TaskStatus.Faulted)
            //  todo 待测试多个时抛错，结果处理
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

        private static TaskContext ConvertToContext(ExcuteReq req, TaskMeta meta, object flowData = null)
        {
            var context = new TaskContext();

            //context.body = (TReq) req.body; //  todo 添加协议转化处理
            //context.flow_data = flowData;
            context.exced_times = 0;
            context.flow_id = req.flow_id;
            context.interval_times = 0;

            context.task_meta = meta;
            return context;
        }

        #endregion

        #region 节点下Task处理

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

                task.TaskMeta = meta;
                taskDirs.Add(meta, task);
            }

            return taskDirs;
        }



        //private static readonly IDictionary<string, BaseTask> _taskContainer = new Dictionary<string, BaseTask>();

        //private static ResultMo<BaseTask> GetTask(string taskKey)
        //{
        //    return _taskContainer.TryGetValue(taskKey, out BaseTask task)
        //        ? new ResultMo<BaseTask>(task)
        //        : new ResultMo<BaseTask>(ResultTypes.UnKnowOperate, "未找到对应的任务处理器！");
        //}

        //private static void RegisteTask<TTask>(string taskKey, bool isSingle = true)
        //    where TTask : BaseTask, new()
        //{
        //    var task = default(TTask);
        //    //if (!InsContainer<TTask>.TryGet(out task))
        //    //{
        //    InsContainer<TTask>.Set<TTask>(isSingle);
        //    task = InsContainer<TTask>.Instance;
        //    //}

        //    RegisteTask(taskKey, task);
        //}

        //private static void RegisteTask(string taskKey, BaseTask task)
        //{
        //    if (_taskContainer.ContainsKey(taskKey))
        //    {
        //        throw new ArgumentException($"have more than one task name with '{taskKey}' in this node!",
        //            nameof(taskKey));
        //    }

        //    _taskContainer.Add(taskKey, task);
        //}

        #endregion

    }

    public abstract class BaseNode
    {
        internal abstract Task<ResultMo> Excute(ExcuteReq fReq, object flowData);
    }

}
