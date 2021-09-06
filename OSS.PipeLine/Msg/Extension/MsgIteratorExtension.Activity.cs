#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class MsgIteratorExtension
    {
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：当前活动上下文（会继续传递给下一个节点）
        /// 结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity<TMsg> SetIteratorWithActivity<TMsgEnumerable, TMsg>(this MsgEnumerator<TMsgEnumerable, TMsg> pipe,
            Func<TMsg, Task<TrafficSignal>> exeFunc, string pipeCode = null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new SimpleActivity<TMsg>( pipeCode, exeFunc);
            pipe.InterSetIterator(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TMsgEnumerable"></typeparam>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc">
        /// 执行委托
        /// 参数：当前活动上下文（会继续传递给下一个节点）
        /// 结果：
        ///     TrafficSignal -（活动是否处理成功，业务结果）
        ///         Green_Pass  - 流体自动流入后续管道
        ///         Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///         Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleActivity<TMsg, TResult> SetIteratorWithActivity<TMsgEnumerable, TMsg, TResult>(this MsgEnumerator<TMsgEnumerable, TMsg> pipe,
            Func<TMsg, Task<TrafficSignal<TResult>>> exeFunc, string pipeCode = null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new SimpleActivity<TMsg, TResult>( pipeCode, exeFunc);
            pipe.InterSetIterator(nextPipe);
            return nextPipe;
        }
    }
}