using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventFlow.Dispatcher;
using OSS.EventFlow.Tasks.Mos;
using OSS.EventFlow.Tasks.Storage;

namespace OSS.EventFlow.Tasks
{
    public abstract class BaseTask<TPara, TRes> 
        where TRes : ResultMo
    {
        private readonly ITaskContextSaver<TPara> _taskSaver;
        public BaseTask(ITaskContextSaver<TPara> taskSaver)
        {
            _taskSaver = taskSaver;
        }

        /// <summary>
        ///   任务重试配置
        /// </summary>
        public TaskRetryConfig RetryConfig { get; set; }

        /// <summary>
        ///  任务编号
        /// </summary>
        public string TaskCode { get; set; }

        #region 具体任务执行入口

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="reqPara"></param>
        /// <returns>  </returns>
        public async Task<ResultMo> Process(TaskContext context, TPara reqPara)
        {
            TRes res;
            var directExcuteTimes = 0;
            do
            {
                //  直接执行
                res = await Do(reqPara);
                if (res == null)
                    throw new ArgumentNullException($"{this.GetType().Name} 任务返回值为空！");

                // 判断是否失败回退
                if (res.IsTaskFailed())
                    await Revert(reqPara);

                directExcuteTimes++;
                context.ExcutedTimes++;

            } // 判断是否执行直接重试 
            while (res.IsTaskFailed() && CheckDirectTryConfig(directExcuteTimes));

            // 判断是否间隔执行,生成重试信息
            if (CheckIntervalTryConfig(context.IntervalTimes++))
            {
                await SaveStack(context, reqPara);
                res.ret = (int) EventFlowResult.WatingRetry;
            }

            if (res.IsTaskFailed())
            {
                //  最终失败，执行失败方法
                await Failed(reqPara);
            }

            return res;
        }

        #endregion

        /// <summary>
        ///  如果需要间隔重试，需要保存当前请求上下文信息
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <param name="context"></param>
        /// <param name="reqPara"></param>
        private Task SaveStack(TaskContext context, TPara reqPara)
        {
            return _taskSaver.SaveTaskContext(context, reqPara);
        }
        
        #region 实现，重试，失败 执行方法

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns>  特殊：ret=-100（EventFlowResult.Failed）  任务处理失败，执行回退，并根据重试设置发起重试</returns>
        protected abstract Task<TRes> Do(TPara req);

        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，调用后重试
        /// </summary>
        /// <param name="req">请求参数</param>
        protected virtual async Task Revert(TPara req)
        {
        }

        /// <summary>
        ///  最终执行失败会执行
        /// </summary>
        /// <param name="req"></param>
        protected virtual async Task Failed(TPara req)
        {
        }

        #endregion

        #region 辅助判断方法

        /// <summary>
        ///  检查是否符合直接重试
        /// </summary>
        /// <param name="directExcuteTimes"></param>
        /// <returns></returns>
        private bool CheckDirectTryConfig(int directExcuteTimes)
        {
            if (directExcuteTimes < RetryConfig?.DirectTimes)
            {
                return true;
            }

            //if (RetryConfig?.RetryType == TaskRetryType.Direct
            //    || RetryConfig?.RetryType == TaskRetryType.DirectThenInterval)
            //{
            //    return true;
            //}

            return false;
        }

        /// <summary>
        ///   判断是否符合间隔重试
        /// </summary>
        /// <param name="intTimes"></param>
        /// <returns></returns>
        private bool CheckIntervalTryConfig(int intTimes)
        {
            if (intTimes < RetryConfig?.IntervalTimes)
            {
                return true;
            }

            //if (RetryConfig?.RetryType == TaskRetryType.Interval
            //    || RetryConfig?.RetryType == TaskRetryType.DirectThenInterval)
            //{
            //    return true;
            //}

            return false;
        }   
        #endregion
    }

    
}