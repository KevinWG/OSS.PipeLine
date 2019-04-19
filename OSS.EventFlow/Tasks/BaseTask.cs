using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventFlow.Dispatcher;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tasks
{
    public abstract class BaseTask<TPara, TRes> 
        where TRes : ResultMo
    {
        #region 具体任务执行入口

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns>  </returns>
        public async Task<ResultMo> Process(TaskContext<TPara> context)
        {
            TRes res;
            var directExcuteTimes = 0;
            do
            {
                //  直接执行
                res = await Do(context.Body);
                if (res == null)
                    throw new ArgumentNullException($"{this.GetType().Name} return null！");

                // 判断是否失败回退
                if (res.IsTaskFailed())
                    await Revert(context.Body);

                directExcuteTimes++;
                context.ExcutedTimes++;
            } 
            // 判断是否执行直接重试 
            while (res.IsTaskFailed() && CheckDirectTryConfig(directExcuteTimes));

            // 判断是否间隔执行,生成重试信息
            if (CheckIntervalTryConfig(context.IntervalTimes++))
            {
                await _contextKepper.Invoke(context);
                res.ret = (int) EventFlowResult.WatingRetry;
            }

            if (res.IsTaskFailed())
            {
                //  最终失败，执行失败方法
                await Failed(context.Body);
            }

            return res;
        }

        #endregion

    
        
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


        #region 重试机制设置


        /// <summary>
        ///   任务重试配置
        /// </summary>
        public TaskRetryConfig RetryConfig { get;private set; }

        /// <summary>
        ///  设置持续重试信息
        /// </summary>
        /// <param name="continueTimes"></param>
        public void SetContinueRetry(int continueTimes)
        {
            if (RetryConfig==null)
                RetryConfig=new TaskRetryConfig();

            RetryConfig.ContinueTimes = continueTimes;
        }


        private Func<TaskContext<TPara>, Task> _contextKepper;
        /// <summary>
        ///  设置持续重试信息
        /// </summary>
        /// <param name="intTimes"></param>
        /// <param name="contextKeeper"></param>
        public void SetIntervalRetry(int intTimes,Func<TaskContext<TPara>,Task> contextKeeper)
        {
            if (RetryConfig == null)
                RetryConfig = new TaskRetryConfig();
            
            RetryConfig.IntervalTimes = intTimes;

            _contextKepper = contextKeeper ?? throw new ArgumentNullException(nameof(contextKeeper), "Context Keeper will save the context info for the next time, can not be null!");
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
            return directExcuteTimes < RetryConfig?.ContinueTimes;
        }

        /// <summary>
        ///   判断是否符合间隔重试
        /// </summary>
        /// <param name="intTimes"></param>
        /// <returns></returns>
        private bool CheckIntervalTryConfig(int intTimes)
        {
            return intTimes < RetryConfig?.IntervalTimes;
        }   
        #endregion
    }

    
}