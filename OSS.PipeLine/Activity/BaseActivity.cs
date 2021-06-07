#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  活动基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    /// 主动触发执行活动组件基类(不接收上下文)
    /// </summary>
    public abstract class BaseActivity : BaseStraightPipe<EmptyContext, EmptyContext>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity() : base(PipeType.Activity)
        {
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        /// 处理结果
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<bool> Executing();

        internal override async Task<bool> InterHandling(EmptyContext context)
        {
            var res = await Executing();
            if (res)
                await ToNextThrough(context);
            
            return res;
        }
    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TContext">输入输出上下文</typeparam>
    public abstract class BaseActivity<TContext> : BaseStraightPipe<TContext, TContext>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseActivity() : base(PipeType.Activity)
        {
        }
        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="contextData">当前活动上下文（会继续传递给下一个节点）</param>
        /// <returns>
        /// 处理结果
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </returns>
        protected abstract Task<bool> Executing(TContext contextData);

        internal override async Task<bool> InterHandling(TContext context)
        {
            var res = await Executing(context);
            if (res)
            {
                await ToNextThrough(context);
            }
            return res;
        }
    }
}
