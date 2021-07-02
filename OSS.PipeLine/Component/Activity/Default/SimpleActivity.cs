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

using System;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    /// 主动触发执行活动组件基类(不接收上下文)
    /// </summary>
    public class SimpleActivity: BaseActivity //: BaseStraightPipe<EmptyContext, EmptyContext>
    {
        private readonly Func<Task<TrafficSignal>> _exePassive;

        /// <inheritdoc />
        public SimpleActivity(string pipeCode, Func<Task<TrafficSignal>> exePassive):base(pipeCode)
        {
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <returns>
        /// 处理结果
        /// False - 触发Block，业务流不再向后续管道传递。
        /// True  - 流体自动流入后续管道
        /// </returns>
        protected override Task<TrafficSignal> Executing()
        {
            return _exePassive();
        }

    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TContext">输入输出上下文</typeparam>
    public class SimpleActivity<TContext> : BaseActivity<TContext>
    {
        private readonly Func<TContext,Task<TrafficSignal>> _exePassive;
        

        /// <inheritdoc />
        public SimpleActivity(string pipeCode, Func<TContext,Task<TrafficSignal>> exePassive):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal> Executing(TContext contextData)
        {
            return _exePassive(contextData);
        }
    }


    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TContext">输入输出上下文</typeparam>
    /// <typeparam name="THandleResult"></typeparam>
    public class SimpleActivity<TContext, THandleResult> : BaseActivity<TContext, THandleResult>
    {
        private readonly Func<TContext, Task<TrafficSignal<THandleResult>>> _exePassive;

        /// <inheritdoc />
        public SimpleActivity(string pipeCode,Func<TContext, Task<TrafficSignal<THandleResult>>> exePassive):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<THandleResult>> Executing(TContext para)
        {
            return _exePassive(para);
        }
    }
}
