using OSS.Common.ComModels;
using OSS.EventFlow.Dispatcher;
using OSS.EventFlow.Tasks.Interfaces;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tasks
{

    //public abstract class BaseTask //where TPara : class
    //{
    //    private TaskOption _config;

    //    /// <summary>
    //    ///   任务重试配置
    //    /// </summary>
    //    public TaskRetryConfig RetryConfig { get; set; }

    //    private ITaskRetryProducer<TPara> _producer;

    //    public BaseTask(TaskOption config)
    //    {
    //        _config = config;
    //    }

    //    public BaseTask(TaskOption config, ITaskRetryProducer<TPara> producer)
    //    {
    //        _config = config;
    //        _producer = producer;
    //    }

    //    /// <summary>
    //    ///     任务的具体执行
    //    /// </summary>
    //    /// <param name="req"></param>
    //    /// <returns>  特殊：ret=-100（EventFlowResult.Failed）  任务处理失败，执行回退，并根据重试设置发起重试</returns>
    //    public abstract TResult Excute(TaskReq<TPara> req);

    //    /// <summary>
    //    ///     任务的具体执行
    //    /// </summary>
    //    /// <param name="req"></param>
    //    /// <returns>  </returns>
    //    internal TResult Do(TaskReq<TPara> req)
    //    {
    //        TResult res;
    //        var directExcuteTimes = 0;
    //        do
    //        {
    //            //  直接执行
    //            res = Excute(req);
    //            // 判断是否失败回退
    //            if (res.IsTaskFailed())
    //                Revert(req);

    //            directExcuteTimes++;
    //            req.ExcutedTimes++;

    //        } // 判断是否执行直接重试 
    //        while (res.IsTaskFailed() && CheckDirectTryConfig(directExcuteTimes));

    //        // 判断是否间隔执行,生成重试信息
    //        if (CheckIntervalTryConfig(req))
    //        {
    //            req.IntervalTimes++;
    //            _producer?.Save(_config, req);

    //            res.ret = (int) EventFlowResult.WatingRetry;
    //        }

    //        return res;
    //    }

    //    /// <summary>
    //    ///  检查是否符合直接重试
    //    /// </summary>
    //    /// <param name="directExcuteTimes"></param>
    //    /// <returns></returns>
    //    private bool CheckDirectTryConfig(int directExcuteTimes)
    //    {
    //        if (directExcuteTimes > RetryConfig?.MaxDirectTimes)
    //        {
    //            return false;
    //        }

    //        if (RetryConfig?.RetryType == TaskRetryType.Direct
    //            || RetryConfig?.RetryType == TaskRetryType.DirectThenInterval)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }

    //    /// <summary>
    //    ///   判断是否符合间隔重试
    //    /// </summary>
    //    /// <param name="req"></param>
    //    /// <returns></returns>
    //    private bool CheckIntervalTryConfig(TaskReq<TPara> req)
    //    {
    //        if (req.IntervalTimes > RetryConfig?.MaxIntervalTimes)
    //        {
    //            return false;
    //        }

    //        if (RetryConfig?.RetryType == TaskRetryType.Interval
    //            || RetryConfig?.RetryType == TaskRetryType.DirectThenInterval)
    //        {
    //            return true;
    //        }

    //        return false;
    //    }


    //    /// <summary>
    //    ///  执行失败回退操作
    //    ///   如果设置了重试配置，会在重试失败后调用
    //    /// </summary>
    //    /// <param name="req">请求参数</param>
    //    protected virtual void Revert(TaskReq<TPara> req)
    //    {
    //    }
    //}


    public abstract class BaseTask<TPara, TResult> //where TPara : class
        where TResult : ResultMo
    {
        private TaskOption _config;

        /// <summary>
        ///   任务重试配置
        /// </summary>
        public TaskRetryConfig RetryConfig { get; set; }

        private ITaskRetryProducer<TPara> _producer;

        public BaseTask(TaskOption config)
        {
            _config = config;
        }

        public BaseTask(TaskOption config, ITaskRetryProducer<TPara> producer)
        {
            _config = config;
            _producer = producer;
        }

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns>  特殊：ret=-100（EventFlowResult.Failed）  任务处理失败，执行回退，并根据重试设置发起重试</returns>
        public abstract TResult Excute(TaskReq<TPara> req);

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns>  </returns>
        internal TResult Do(TaskReq<TPara> req)
        {
            TResult res;
            var directExcuteTimes = 0;
            do
            {
                //  直接执行
                res = Excute(req);
                // 判断是否失败回退
                if (res.IsTaskFailed())
                    Revert(req);

                directExcuteTimes++;
                req.ExcutedTimes++;

            } // 判断是否执行直接重试 
            while (res.IsTaskFailed() && CheckDirectTryConfig(directExcuteTimes));

            // 判断是否间隔执行,生成重试信息
            if (CheckIntervalTryConfig(req))
            {
                req.IntervalTimes++;
                _producer?.Save(_config, req);

                res.ret = (int) EventFlowResult.WatingRetry;
            }

            return res;
        }

        /// <summary>
        ///  检查是否符合直接重试
        /// </summary>
        /// <param name="directExcuteTimes"></param>
        /// <returns></returns>
        private bool CheckDirectTryConfig(int directExcuteTimes)
        {
            if (directExcuteTimes > RetryConfig?.MaxDirectTimes)
            {
                return false;
            }

            if (RetryConfig?.RetryType == TaskRetryType.Direct
                || RetryConfig?.RetryType == TaskRetryType.DirectThenInterval)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///   判断是否符合间隔重试
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool CheckIntervalTryConfig(TaskReq<TPara> req)
        {
            if (req.IntervalTimes > RetryConfig?.MaxIntervalTimes)
            {
                return false;
            }

            if (RetryConfig?.RetryType == TaskRetryType.Interval
                || RetryConfig?.RetryType == TaskRetryType.DirectThenInterval)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，会在重试失败后调用
        /// </summary>
        /// <param name="req">请求参数</param>
        protected virtual void Revert(TaskReq<TPara> req)
        {
        }
    }

    //public abstract class BaseTask
    //{
    //    public BaseTask(TaskConfig config)
    //    {
    //    }

    //    /// <summary>
    //    ///     任务的具体执行
    //    /// </summary>
    //    /// <param name="doTimes"> 执行次数 </param>
    //    public abstract ResultMo<object> Excute(int doTimes = 0);


    //    /// <summary>
    //    ///  执行失败回退
    //    ///   如果设置了重试配置，会在重试失败后调用
    //    /// </summary>
    //    public void Revert(int doTimes = 0)
    //    {
    //    }
    //}
}