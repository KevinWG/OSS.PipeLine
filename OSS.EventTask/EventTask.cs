#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 单事件任务
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
    public abstract  class EventTask<TTData, TTRes> 
        : BaseEventTask<EventTaskMeta, TTData, EventTaskResp<TTRes>>, IEventTask<TTData, TTRes>
    {
        protected EventTask():this(null)
        {
        }

        protected EventTask(EventTaskMeta meta) : base(meta)
        {
            OriginType = EventElementType.Task;
        }

        #region 扩展方法

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        ///    【仅在 OwnerType = OwnerType.Task 时发生】
        ///     节点下的环境信息，由节点内部处理,防止节点其他耗时任务造成执行过程中发起重试操作 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="taskResp"></param>
        /// <returns></returns>
        protected virtual Task SaveTaskContext(TTData data, EventTaskResp<TTRes> taskResp)
        {
            return Task.CompletedTask;
        }

        #endregion



        #region 扩展方法（实现，回退，失败）  扩展方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="data"></param>
        /// <param name="loopTimes">内部循环执行次数</param>
        /// <param name="triedTimes">重试运行次数</param>
        /// <returns> 
        ///  runStatus = TaskRunStatus.RunFailed 系统会字段判断是否满足重试条件执行重试
        /// </returns>
        protected abstract Task<DoResp<TTRes>> Do(TTData data, int loopTimes, int triedTimes);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="data"></param>
        public virtual Task<bool> Revert(TTData data)
        {
            return Task.FromResult(true);
        }


        #endregion

        #region 辅助方法

        internal override async Task Processing(TTData data, EventTaskResp<TTRes> taskResp)
        {
            // 【1】 执行起始方法 附加校验
            var checkRes = RunCheck(taskResp.meta);
            if (!checkRes)
                return;
            do
            {
                var doResp = await Do(data, taskResp.loop_times, taskResp.tried_times);
                doResp.SetToTaskResp(taskResp);

                // 判断是否失败回退
                if (doResp.run_status.IsFailed()
                    && (taskResp.meta.revert_effect == RevertEffect.RevertSelf
                        || taskResp.meta.revert_effect == RevertEffect.RevertSelf))
                {
                    await Revert(data);
                    taskResp.has_reverted = true;
                }
                // 【3】 执行结束方法
                await ProcessEnd(data, taskResp);

                taskResp.loop_times++;
            } while (taskResp.run_status.IsFailed() && taskResp.loop_times <= taskResp.meta.loop_times);
        }

        private static bool RunCheck(EventTaskMeta taskMeta)
        {
            if (string.IsNullOrEmpty(taskMeta?.task_id))
            {
                throw new ArgumentNullException("Task metainfo is null!");
            }

            return true;
        }

        #endregion

  
    }
}