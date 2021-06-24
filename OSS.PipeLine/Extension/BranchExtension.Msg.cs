#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  分支扩展
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

namespace OSS.Pipeline
{
    /// <summary>
    ///  网关扩展类
    /// </summary>
    public static partial class BranchExtension
    {
        /// <summary>
        ///  追加消息发布者管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static void AddMsgPublisherBranch<TContext>(this BaseBranchGateway<TContext> pipe, string msgFlowKey, DataPublisherOption option=null)
        {
            var nextPipe = new MsgPublisher<TContext>(msgFlowKey, option);
            pipe.AddBranch(nextPipe);
        }

        /// <summary>
        ///  追加消息流管道
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="msgFlowKey">消息flowKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseMsgFlow<TContext> AddMsgFlowBranch<TContext>(this BaseBranchGateway<TContext> pipe, string msgFlowKey, DataFlowOption option=null)
        {
            var nextPipe = new MsgFlow<TContext>(msgFlowKey, option);
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
        public static BaseMsgConverter<TContext, NextOutContext> AddMsgConverterBranch<TContext, NextOutContext>(
            this BaseBranchGateway<TContext> pipe, Func<TContext, NextOutContext> convertFunc,string pipeCode=null)
        {
            var nextPipe = new InterMsgConvertor<TContext, NextOutContext>(convertFunc, pipeCode);
            pipe.AddBranch(nextPipe);
            return nextPipe;
        }

    }
}