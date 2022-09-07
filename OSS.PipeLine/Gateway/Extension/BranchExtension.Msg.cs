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

using OSS.DataFlow;
using System;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class BranchExtension
    {
        /// <summary>
        ///  追加默认消息发布者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="msgDataKey">消息pipeDataKey，默认消息实现对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleMsgPublisher<TMsg> AppendMsgPublisher<TMsg>(
            this IBranchGateway<TMsg> pipe, Func<TMsg, bool> branchCondition, string msgDataKey,
            DataPublisherOption option = null, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgPublisher<TMsg>(msgDataKey, option, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息发布者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="pushKeyGenerator">消息key生成器,为空则使用pipeCode作为发布消息key</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static SimpleMsgPublisher<TMsg> AppendMsgPublisher<TMsg>(
            this IBranchGateway<TMsg> pipe, Func<TMsg, bool> branchCondition, Func<TMsg, string> pushKeyGenerator,
            DataPublisherOption option = null, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgPublisher<TMsg>(pushKeyGenerator, option, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息订阅者管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="msgDataKey">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgSubscriber<TMsg> AppendMsgSubscriber<TMsg>(
            this IBranchGateway<TMsg> pipe, Func<TMsg, bool> branchCondition, string msgDataKey,
            string pipeCode = null)
        {
            var nextPipe = new SimpleMsgSubscriber<TMsg>(msgDataKey, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息流管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="msgDataKey">消息pipeDataKey，默认对应的flow是异步线程池</param>
        /// <param name="option"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgFlow<TMsg> AppendMsgFlow<TMsg>(
            this IBranchGateway<TMsg> pipe, Func<TMsg, bool> branchCondition, string msgDataKey,
            DataFlowOption option = null, string pipeCode = null)
        {
            var nextPipe = new SimpleMsgFlow<TMsg>(msgDataKey, option, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }

        /// <summary>
        ///  追加默认消息转换管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="branchCondition">分支条件判断</param>
        /// <param name="convertFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseMsgConverter<TMsg, NextOutContext> AppendMsgConverter<TMsg, NextOutContext>(
            this IBranchGateway<TMsg> pipe, Func<TMsg, bool> branchCondition, Func<TMsg, NextOutContext> convertFunc,
            string pipeCode = null)
        {
            var nextPipe = new SimpleMsgConvertor<TMsg, NextOutContext>(convertFunc, pipeCode);

            pipe.SetCondition(nextPipe, branchCondition);
            pipe.InterAppend(nextPipe);

            return nextPipe;
        }
        
    }
}