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
    ///  基础工作节点
    /// todo  重新激活处理
    /// todo  协议处理
    /// todo   flowentitytype
    /// </summary>
    public abstract partial class BaseNode<TReq> // : INode<TReq>,IFlowNode
    {
        
        #region 节点具体执行方法

        public async Task<ResultMo> Excuting(NodeContext con)
        {
            // 获取任务元数据列表
            var taskMetaRes = await GetTaskMetas(con);
            if (!taskMetaRes.IsSuccess())
                return new ResultMo(ResultTypes.UnKnowOperate, "未知的处理操作！");
            var taskMetas = taskMetaRes.data;

            // 获取元信息对应任务处理实例
            var taskDirs = GetTasks(taskMetas);

            // 执行处理结果
            var taskResults = await Excuting_Results(con, taskDirs);

            // 处理结束后加工处理
            return await Excute_End(taskResults);
        }
        
        #region 节点执行过程中分解方法

    
        private async Task<Dictionary<TaskMeta, ResultMo>> Excuting_Results(NodeContext req,
            IDictionary<TaskMeta, BaseTask> taskDirs)
        {
            Dictionary<TaskMeta, ResultMo> taskResults;

            if (NodeMeta.node_type == NodeType.Parallel)
            {
                taskResults = Excuting_Parallel(req, taskDirs);
            }
            else
            {
                taskResults = await Excuting_Sequence(req, taskDirs);
            }

            return taskResults;
        }

        /// <summary>
        ///  顺序执行
        /// </summary>
        /// <param name="req"></param>
        /// <param name="taskDirs"></param>
        /// <returns></returns>
        private static async Task<Dictionary<TaskMeta, ResultMo>> Excuting_Sequence(NodeContext req,
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
        private static Dictionary<TaskMeta, ResultMo> Excuting_Parallel(NodeContext req,
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


        private static TaskContext ConvertToContext(NodeContext req, TaskMeta meta)
        {
            var context = req.ConvertToTaskContext();

            //context.body = (TReq) req.body; //  todo 添加协议转化处理
            //context.flow_data = flowData;
            context.task_meta = meta;

            return context;
        }


        #endregion
        
        #endregion


        #region 节点完成后执行


        protected virtual Task<ResultMo> Excute_End(Dictionary<TaskMeta, ResultMo> taskResults)
        {
            var res = taskResults.FirstOrDefault(p => p.Value.sys_ret != 0).Value;
            return Task.FromResult(res ?? new ResultMo());
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
        #endregion

    }
}
