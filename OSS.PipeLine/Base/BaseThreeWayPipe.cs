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


using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道执行基类（主动三向类型 ）
    ///   输入：上游传递的上下文
    ///   输出：主动结果输出， 下游上下文参数输出
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public abstract class BaseThreeWayPipe<TInContext, THandleResult, TOutContext> : BaseFourWayPipe<TInContext, TInContext, THandleResult, TOutContext>,
        IPipeExecutor<TInContext, THandleResult>
    {
        /// <inheritdoc />
        protected BaseThreeWayPipe(string pipeCode, PipeType pipeType) : base(pipeCode, pipeType)
        {
        }

        #region 流体外部扩展

        /// <summary>
        /// 外部执行方法 - 启动入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<THandleResult> Execute(TInContext para)
        {
            return (await InterProcess(para, string.Empty)).result;
        }
        
        #endregion


        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(TInContext context, string prePipeCode)
        {
            await Watch(PipeCode, PipeType, WatchActionType.PreCall, context).ConfigureAwait(false);
            var tRes = await InterProcess(context,prePipeCode);
            return tRes.ToResult();
        }

        #endregion
    }
}