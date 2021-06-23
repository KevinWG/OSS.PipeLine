using OSS.Pipeline.Interface;
using System;
using System.Threading.Tasks;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类（空参被动类型）
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncPipe<TFuncPara,TFuncResult, TOut> :
       BasePipe<Empty, TFuncPara, TFuncResult, TOut>,IPipeFuncExecutor<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncPipe(PipeType pipeType) : base(pipeType)
        {
        }

        /// <summary>
        ///  启动
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<TFuncResult> Execute(TFuncPara para)
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
