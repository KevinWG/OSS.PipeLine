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
using OSS.DataFlow;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Msg;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class PipelineFactory
    {
        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipeCode">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipelineAppender<Empty,OutContext> StartWithMsgSubscriber<OutContext>( string pipeCode, DataFlowOption option = null)
        {
            var nextPipe = new MsgSubscriber<OutContext>(pipeCode, option);
            
            return Start(nextPipe);
        }


        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipeCode">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipelineAppender<OutContext, OutContext> StartWithMsgFlow<OutContext>( string pipeCode, DataFlowOption option = null)
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
        public static IPipelineAppender<OutContext, NextOutContext> StartWithMsgConverter<OutContext, NextOutContext>( Func<OutContext, NextOutContext> convertFunc, string pipeCode = null)
        {
            var nextPipe = new InterMsgConvertor<OutContext, NextOutContext>(convertFunc, pipeCode);
            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }
            return Start(nextPipe);
        }

    }
}
