using OSS.Pipeline.Interface;
using System;
using System.Threading.Tasks;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类（被动三向类型 ）
    ///   输入：被动入参 （隐形忽略上游传参
    ///   输出：被动结果输出， 下游上下文参数输出
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="THandlePara"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseThreeWayFuncPipe<THandlePara,THandleResult, TOut> :
       BaseFourWayPipe<Empty, THandlePara, THandleResult, TOut>,IPipeFuncExecutor<THandlePara, THandleResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseThreeWayFuncPipe(PipeType pipeType) : base(pipeType)
        {
        }

        /// <summary>
        ///  启动
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<THandleResult> Execute(THandlePara para)
        {
            var trafficRes = await InterProcess(para);
            return trafficRes.result;
        }
        
        #region 内部的业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(Empty context)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Starting, context);
            return  new TrafficResult(SignalFlag.Green_Pass,String.Empty, String.Empty);
        }
        
        #endregion



    }
}
