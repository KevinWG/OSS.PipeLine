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

using OSS.Pipeline.InterImpls;
using System.Threading.Tasks;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类（被动三向类型 ）
    ///   输入：被动入参 （隐形忽略上游传参
    ///   输出：被动结果输出， 下游上下文参数输出
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TPara"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BaseThreeWayPassivePipe<TPara,TRes, TOut> :
       BaseFourWayPipe<Empty, TPara, TRes, TOut> //,IPipeExecutor<TPara, TRes>
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
        public async Task<TRes> Execute(TPara para)
        {
            var trafficRes = await InterProcess(para);
            return trafficRes.result;
        }
        
        #region 内部的业务处理

        /// <inheritdoc />
        internal override Task<TrafficSignal> InterPreCall(Empty context)
        {
            return InterUtil.GreenTrafficSignalTask;
        }
        
        #endregion
    }
}
