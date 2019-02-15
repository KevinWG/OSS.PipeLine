using OSS.Common.ComModels;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tasks
{
   
    public abstract class BaseTask<TPara, TResult> //where TPara : class
        where TResult : ResultMo
    {
        public BaseTask(TaskConfig config)
        {
        }

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns>  特殊：ret=-100（EventFlowResult.Failed）  任务处理失败，执行判断重新执行</returns>
        public abstract TResult Excute(BaseTaskReq<TPara> req);

        /// <summary>
        ///     任务的具体执行
        /// </summary>
        /// <param name="req"></param>
        /// <returns>  </returns>
        internal TResult Do(BaseTaskReq<TPara> req)
        {
            return Excute(req);
        }


        /// <summary>
        ///  执行失败回退操作
        ///   如果设置了重试配置，会在重试失败后调用
        /// </summary>
        /// <param name="req">请求参数</param>
        public void Revert(BaseTaskReq<TPara> req)
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