#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 事件任务基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.EventTask.Extension;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract class BaseEventTask<TMetaType, TTData, TResp> : BaseMeta<TMetaType>,IBufferTunnel<TTData,TResp>
        where TMetaType : BaseTaskMeta
        //where TTData : class
        where TResp : BaseTaskResp<TMetaType>, new()
    {

        /// <summary>
        ///   归属类型
        /// </summary>
        public EventElementType OwnerType { get; internal set; } = EventElementType.Task;
        internal EventElementType OriginType { get;  set; } = EventElementType.Task;

        protected BaseEventTask()
        {
        }

        protected BaseEventTask(TMetaType meta) : base(meta)
        {
        }

        #region 扩展方法

        /// <summary>
        ///  阻塞 -  保存对应运行请求和重试相关信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resp"></param>
        /// <returns></returns>
        public virtual Task Push(TTData data, TResp resp)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 重新唤起
        /// </summary>
        /// <param name="data"></param>
        /// <param name="triedTimes"></param>
        /// <returns></returns>
        public Task Pop(TTData data, int triedTimes)
        {
            return Process(data, triedTimes);
        }

        #endregion

        #region 任务进入入口

        public Task<TResp> Process(TTData data) => Process(data, 0);

        public async Task<TResp> Process(TTData data, int triedTimes)
        {
            var meta = await GetMeta();
            var taskResp = new TResp
            {
                tried_times = triedTimes,
                run_status = TaskRunStatus.WaitToRun,
                meta = meta
            };

            await Processing(data, taskResp);

            taskResp.executed_time = DateTime.Now.ToUtcSeconds();

            // 判断是否间隔执行,生成重试信息
            // 任务被加入群组之后，间歇重试功能由群组承接
            if (taskResp.run_status.IsFailed()
                && taskResp.tried_times < taskResp.meta.retry_times && OwnerType == OriginType)
            {
                taskResp.tried_times++;
                taskResp.next_time = taskResp.executed_time + taskResp.meta.retry_seconds;

                taskResp.run_status = TaskRunStatus.RunPaused;

                await Push(data, taskResp);
            }

            //  最终失败，执行失败方法
            if (taskResp.run_status.IsFailed())
                await FinallyFailed(data, taskResp);

            return taskResp;
        }


        #endregion


        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res"></param>
        /// <returns> 
        ///  runStatus = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </returns>
        internal abstract Task Processing(TTData data, TResp res);
        

        /// <summary>
        /// 任务结束方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res">请求的上下文</param>
        /// <returns></returns>
        protected virtual Task ProcessEnd(TTData data, TResp res)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        ///  最终失败执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="res"></param>
        protected virtual Task FinallyFailed(TTData data, TResp res)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
