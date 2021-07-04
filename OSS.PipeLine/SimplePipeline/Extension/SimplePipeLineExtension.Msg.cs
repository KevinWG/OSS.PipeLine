#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  流体扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System;
using OSS.DataFlow;
using OSS.Pipeline.InterImpls.Msg;
using OSS.Pipeline.SimplePipeline.InterImpls.Connector;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class SimplePipelineExtension
    {

        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> ThenWithMsgFlow<TContext>(this ISimplePipelineConnector<TContext> pipe, 
            string pipeCode, DataFlowOption option = null)
        {
            var nextPipe = new MsgFlow<TContext>(pipeCode, option);

            return pipe.Then(nextPipe);
        }
        
    }
}
