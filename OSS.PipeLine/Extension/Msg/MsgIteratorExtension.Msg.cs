#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道扩展-消息类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using OSS.DataFlow;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class MsgIteratorExtension
    {
        /// <summary>
        ///  追加默认消息发布者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgEnumerator<TMsgEnumerable,TMsg> SetMsgPublisherIterator<TMsgEnumerable, TMsg>(this BaseMsgEnumerator<TMsgEnumerable, TMsg> pipe, string pipeCode,
            DataPublisherOption option = null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new MsgPublisher<TMsg>(pipeCode, option);
            pipe.InterSetIterator(nextPipe);
            return pipe;
        }
        
        ///// <summary>
        /////  追加默认消息流管道
        ///// </summary>
        ///// <typeparam name="TMsg"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        ///// <param name="option"></param>
        ///// <returns></returns>
        //public static MsgEnumerator<TMsg> SetMsgFlowIterator<TMsg>(this MsgEnumerator<TMsg> pipe, string pipeCode,
        //    DataFlowOption option = null)
        //{
        //    var nextPipe = new MsgFlow<TMsg>(pipeCode, option);

        //    pipe.InterSetIterator(nextPipe);
        //    return pipe;
        //}

        ///// <summary>
        /////  追加默认消息转换管道
        ///// </summary>
        ///// <typeparam name="TMsg"></typeparam>
        ///// <typeparam name="NextOutContext"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="convertFunc"></param>
        ///// <param name="pipeCode"></param>
        ///// <returns></returns>
        //public static MsgEnumerator<TMsg> SetMsgConverterIterator<TMsg, NextOutContext>(
        //    this MsgEnumerator<TMsg> pipe, string pipeCode, Func<TMsg, NextOutContext> convertFunc)
        //{
        //    var nextPipe = new InterMsgConvertor<TMsg, NextOutContext>(pipeCode,convertFunc);
        //    pipe.InterSetIterator(nextPipe);
        //    return pipe;
        //}
    }
}