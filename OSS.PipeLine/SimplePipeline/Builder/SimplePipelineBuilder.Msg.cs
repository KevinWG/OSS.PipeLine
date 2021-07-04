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

using OSS.DataFlow;
using OSS.Pipeline.SimplePipeline.InterImpls.Connector;

namespace OSS.Pipeline
{
    /// <summary>
    ///  pipeline 生成器
    /// </summary>
    public static partial class SimplePipelineBuilder
    {
        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipeCode">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static ISimplePipelineConnector<TContext> StartWithMsgFlow<TContext>(string pipeCode, DataFlowOption option = null)
        {
            var nextPipe = new MsgFlow<TContext>(pipeCode, option);

            return Start(nextPipe);
        }
    }
}
