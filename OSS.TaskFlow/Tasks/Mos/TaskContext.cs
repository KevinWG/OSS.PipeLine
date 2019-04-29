using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.MetaMos;

namespace OSS.TaskFlow.Tasks.Mos
{
    /// <summary>
    ///  任务上下文基类
    /// </summary>
    public class TaskContext:NodeContext
    {

        /// <summary>
        ///  task元信息
        /// </summary>
        public TaskMeta  task_meta { get; set; }
        
        /// <summary>
        ///  执行总次数
        /// </summary>
        public int exced_times { get;internal set; }
        
        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int interval_times { get; internal set; }

        /// <summary>
        ///  上次执行时间
        /// </summary>
        public long last_excutedate { get; set; }
    }

    public static class TaskContextExtention
    {
        public static TaskContext ConvertToTaskContext(this NodeContext node)
        {
            var taskCon=new TaskContext();

            taskCon.run_id = node.run_id;
            taskCon.flow_meta = node.flow_meta;
            taskCon.node_meta = node.node_meta;

            return taskCon;
        }

        public static async Task<ResultMo> CheckTaskContext(this TaskContext context,InstanceType insType, Func<Task<ResultIdMo>> idGenerater)
        {
            var res = insType == InstanceType.Stand
                ? new ResultMo()
                : await context.CheckNodeContext(insType, idGenerater);
            if (!res.IsSysOk())
                return res;

            if (string.IsNullOrEmpty(context.task_meta?.task_key))
            {
                res.sys_ret = (int)SysResultTypes.ConfigError;
                res.ret = (int)ResultTypes.InnerError;
                res.msg = "task metainfo has error!";
                return res;
            }

            if (!string.IsNullOrEmpty(context.run_id))
                return res;

            var idRes = await idGenerater.Invoke();
            if (!idRes.IsSuccess())
                return idRes;

            context.run_id = idRes.id;
            return res;
        
        }
    }
}
