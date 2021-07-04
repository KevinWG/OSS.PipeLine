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

using System;
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
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="pushKeyGenerator"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgEnumerator<TMsgEnumerable,TMsg> SetMsgPublisherIterator<TMsgEnumerable, TMsg>(this BaseMsgEnumerator<TMsgEnumerable, TMsg> pipe, string pipeCode,
            Func<TMsg,string> pushKeyGenerator=null, DataPublisherOption option = null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new MsgPublisher<TMsg>(pipeCode, pushKeyGenerator, option);
            pipe.InterSetIterator(nextPipe);
            return pipe;
        }
    }
}