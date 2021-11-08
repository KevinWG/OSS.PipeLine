#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Interface;
using System;
using System.Threading.Tasks;

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
    public abstract class BaseThreeWayPassivePipe<THandlePara,THandleResult, TOut> :
       BaseFourWayPipe<Empty, THandlePara, THandleResult, TOut>,IPipeExecutor<THandlePara, THandleResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseThreeWayPassivePipe(string pipeCode,PipeType pipeType) : base(pipeCode,pipeType)
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
        internal override  Task<TrafficResult> InterPreCall(Empty context)
        {
            return Task.FromResult(new TrafficResult(SignalFlag.Green_Pass, string.Empty, string.Empty));
        }
        
        #endregion
    }
}
