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

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道执行基类（拦截类型）
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public abstract class BaseInterceptPipe<TInContext> : BaseInPipePart<TInContext>
    {
        /// <inheritdoc />
        protected BaseInterceptPipe(PipeType pipeType) : base(pipeType)
        {
        }
        
        #region 流体业务处理

        internal override Task<bool> InterStart(TInContext context)
        {
            return InterIntercept(context);
        }


        internal abstract Task<bool> InterIntercept(TInContext context);

        #endregion





        #region 内部初始化扩散方法

        internal override void InterInitialContainer(IPipeLine containerFlow)
        {
            throw new System.NotImplementedException($"{PipeCode} 当前的内部 InterInitialContainer 方法没有实现，无法执行");
        }

        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            throw new System.NotImplementedException($"{PipeCode} 当前的内部 InterToRoute 方法没有实现，无法执行");
        }

        #endregion
    }
}