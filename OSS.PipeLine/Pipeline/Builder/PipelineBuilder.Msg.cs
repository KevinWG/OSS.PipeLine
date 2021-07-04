#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体生成器
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
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Msg;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class PipelineBuilder
    {
        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipelineConnector<Empty,OutContext> StartWithMsgSubscriber<OutContext>( string pipeCode, DataFlowOption option = null)
        {
            var nextPipe = new MsgSubscriber<OutContext>(pipeCode, option);
            
            return Start(nextPipe);
        }


        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipelineConnector<OutContext, OutContext> StartWithMsgFlow<OutContext>( string pipeCode, DataFlowOption option = null)
        {
            var nextPipe = new MsgFlow<OutContext>(pipeCode, option);

            return Start(nextPipe);
        }

        /// <summary>
        ///  追加默认消息转换管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="convertFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static IPipelineConnector<OutContext, NextOutContext> StartWithMsgConverter<OutContext, NextOutContext>( Func<OutContext, NextOutContext> convertFunc, string pipeCode = null)
        {
            var nextPipe = new InterMsgConvertor<OutContext, NextOutContext>(pipeCode,convertFunc);
            return Start(nextPipe);
        }



        /// <summary>
        ///  追加消息枚举器
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <returns></returns>
        public static IPipelineMsgEnumerableConnector<TMsgEnumerable,TMsgEnumerable, TMsg> StartWithMsgEnumerator<TMsgEnumerable,TMsg>(string pipeCode=null)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            var nextPipe = new BaseMsgEnumerator<TMsgEnumerable,TMsg>(pipeCode);
            return Start(nextPipe);
        }

    }
}
