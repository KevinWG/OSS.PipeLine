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
    /// todo  全部节点回退
    /// </summary>
    public abstract partial class BaseNode<TReq,TRes> 
        where TRes : ResultMo, new()
    {
        #region 前置进入方法

        /// <summary>
        ///  前置进入方法
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task MoveIn(NodeContext con)
        {
            return Task.CompletedTask;
        }


        #endregion
        
        #region 节点执行
        //  执行
        internal async Task<TRes> Excute(NodeContext con, TaskReqData<TReq> req)
        {
            //  检查初始化
            var checkRes = con.CheckNodeContext();
            if (!checkRes.IsSysOk())
                return checkRes.ConvertToResultInherit<TRes>();
            // 【1】 扩展前置执行方法
            await ExcutePreInternal(con, req);

            // 【2】 任务处理执行方法
            var taskResults = await Excuting(con, req);
            if (!taskResults.IsSuccess())
                return taskResults.ConvertToResultInherit<TRes>();
          
            var nodeRes = await ExcuteResult(con, req, taskResults.data);   // 任务结果加工处理

            //  【3】 扩展后置执行方法
            await ExcuteEnd(nodeRes, taskResults.data, con);

            return nodeRes;
        }



        internal virtual Task ExcutePreInternal(NodeContext con, TaskReqData<TReq> req)
        {
            return Task.CompletedTask;
        }

        protected virtual Task ExcuteEnd(TRes nodeRes, Dictionary<TaskMeta, ResultMo> tastItemDirs, NodeContext con)
        {
            return Task.CompletedTask;
        }


        #endregion

        #region 节点内部任务执行方法

        private async Task<ResultMo<Dictionary<TaskMeta, ResultMo>>> Excuting(NodeContext con, TaskReqData<TReq> req)
        {
            
            // 获取任务元数据列表
            var taskMetaRes = await GetTaskMetas(con);
            if (!taskMetaRes.IsSuccess())
                return taskMetaRes.ConvertToResult<Dictionary<TaskMeta, ResultMo>>();
                //return new ResultMo<Dictionary<TaskMeta, ResultMo>>()
                //{
                //    sys_ret = (int)TaskResultType.ConfigError,
                //    ret =(int)ResultTypes.UnKnowOperate,msg= "未知的处理操作！"
                //};

            var taskMetas = taskMetaRes.data;

            // 获取元信息对应任务处理实例
            var taskDirs = GetTasks(taskMetas);

            // 执行处理结果
            var taskResults = await ExcutingWithTasks(con, req, taskDirs);
            return new ResultMo<Dictionary<TaskMeta, ResultMo>>(taskResults);
 
        }

        #region 节点执行过程中分解方法


        private async Task<Dictionary<TaskMeta, ResultMo>> ExcutingWithTasks(NodeContext con, TaskReqData<TReq> req,
            IDictionary<TaskMeta, BaseTask<TReq>> taskDirs)
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
        private static async Task<Dictionary<TaskMeta, ResultMo>> Excuting_Sequence(NodeContext con, TaskReqData<TReq> req,
            IDictionary<TaskMeta, BaseTask<TReq>> taskDirs)
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
        private static Dictionary<TaskMeta, ResultMo> Excuting_Parallel(NodeContext con, TaskReqData<TReq> req,
            IDictionary<TaskMeta, BaseTask<TReq>> taskDirs)
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
            catch{}
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

        //  todo 修改至其它方法
        protected virtual Task<TRes> ExcuteResult(NodeContext con, TaskReqData<TReq> req, Dictionary<TaskMeta, ResultMo> taskResDirs)
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
                {
                    tRes = res;
                }

            }
            if (tRes==null)
            {
                throw new ArgumentNullException($"can't find a task of return value match node({this.GetType()}) of return value!");
            }
            
            return Task.FromResult(tRes);
        }

     
        
        #endregion
        
        #region 节点下Task处理
        private IDictionary<TaskMeta, BaseTask<TReq>> GetTasks(IList<TaskMeta> metas)
        {
            var taskDirs = new Dictionary<TaskMeta, BaseTask<TReq>>(metas.Count);
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

    }
}
