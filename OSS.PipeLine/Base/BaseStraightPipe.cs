#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基础管道部分
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
    ///  管道执行基类（直通类型）
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public abstract class BaseStraightPipe<TInContext, TOutContext> : BasePipe<TInContext, TInContext, TOutContext, TOutContext>,
        IPipeExecutor<TInContext,TOutContext>
    {
        /// <inheritdoc />
        protected BaseStraightPipe(PipeType pipeType) : base(pipeType)
        {
        }

        #region 流体外部扩展

        /// <summary>
        /// 外部执行方法 - 启动入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<TOutContext> Execute(TInContext para)
        {
            return (await InterProcess(para)).result;
        }

        #endregion


        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(TInContext context)
        {
            await Watch(PipeCode, PipeType, WatchActionType.Starting, context);
            var tRes = await InterProcess(context);
            return tRes.ToResult();
        }


        #endregion



    }
}