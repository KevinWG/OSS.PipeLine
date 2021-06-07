using System;
using OSS.DataFlow;
using OSS.Pipeline.InterImpls.Msg;

namespace OSS.Pipeline
{
    /// <summary>
    ///  网关扩展类
    /// </summary>
    public static class BranchGatewayExtension
    {
        /// <summary>
        ///  追加消息发布者管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static void AppendMsgPublisher<TContext>(this BaseBranchGateway<TContext> pipe, string msgFlowKey, string pipeCode = null, DataPublisherOption option=null)
        {
            var nextPipe = new InterMsgPublisher<TContext>(msgFlowKey, option);

            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }

            pipe.AddBranch(nextPipe);
        }

        /// <summary>
        ///  追加消息流管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgFlow<TContext> AppendMsgFlow<TContext>(this BaseBranchGateway<TContext> pipe, string msgFlowKey, string pipeCode = null, DataFlowOption option=null)
        {
            var nextPipe = new InterMsgFlow<TContext>(msgFlowKey, option);

            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }

            pipe.AddBranch(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加消息转换管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="convertFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgConverter<TContext, NextOutContext> AppendMsgConverter<TContext, NextOutContext>(
            this BaseBranchGateway<TContext> pipe, Func<TContext, NextOutContext> convertFunc,string pipeCode=null)
        {
            var nextPipe = new InterMsgConvertor<TContext, NextOutContext>(convertFunc);

            if (!string.IsNullOrEmpty(pipeCode))
            {
                nextPipe.PipeCode = pipeCode;
            }

            pipe.AddBranch(nextPipe);
            return nextPipe;
        }

    }
}