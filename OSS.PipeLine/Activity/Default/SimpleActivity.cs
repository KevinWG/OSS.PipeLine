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
    public class SimpleActivity: BaseActivity 
    {
        private readonly Func<Task<TrafficSignal>> _exeFunc;

        /// <inheritdoc />
        public SimpleActivity( Func<Task<TrafficSignal>> exeFunc, string pipeCode = null) :base(pipeCode)
        {
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
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
            return _exeFunc();
        }

    }

    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TIn">输入输出上下文</typeparam>
    public class SimpleActivity<TIn> : BaseActivity<TIn>
    {
        private readonly Func<TIn,Task<TrafficSignal>> _exeFunc;
        

        /// <inheritdoc />
        public SimpleActivity( Func<TIn,Task<TrafficSignal>> exeFunc, string pipeCode = null) :base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal> Executing(TIn contextData)
        {
            return _exeFunc(contextData);
        }
    }


    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TIn">输入输出类型</typeparam>
    /// <typeparam name="TRes"></typeparam>
    public class SimpleActivity<TIn, TRes> : BaseActivity<TIn, TRes>
    {
        private readonly Func<TIn, Task<TrafficSignal<TRes>>> _exeFunc;

        /// <inheritdoc />
        public SimpleActivity(Func<TIn, Task<TrafficSignal<TRes>>> exeFunc, string pipeCode = null) :base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TRes>> Executing(TIn para)
        {
            return _exeFunc(para);
        }
    }


    /// <summary>
    ///  主动触发执行活动组件基类
    ///    接收输入上下文，且此上下文继续传递下一个节点
    /// </summary>
    /// <typeparam name="TIn">输入类型</typeparam>
    /// <typeparam name="TRes">返回结果类型</typeparam>
    /// <typeparam name="TOut">输出类型</typeparam>
    public class SimpleActivity<TIn, TRes,TOut> : BaseActivity<TIn, TRes, TOut>
    {
        private readonly Func<TIn, Task<TrafficSignal<TRes, TOut>>> _exeFunc;

        /// <inheritdoc />
        public SimpleActivity(Func<TIn, Task<TrafficSignal<TRes, TOut>>> exeFunc, string pipeCode=null) : base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }

        /// <inheritdoc />
        protected override Task<TrafficSignal<TRes, TOut>> Executing(TIn para)
        {
            return _exeFunc(para);
        }
    }
}
