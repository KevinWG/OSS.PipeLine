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

using OSS.DataFlow.Event;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextOut"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextRes"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseFourWayPipe<TOut, TNextPara, TNextRes, TNextOut> Append<TOut, TNextPara, TNextRes, TNextOut>(
            this IPipeAppender<TOut> pipe, 
            BaseFourWayPipe<TOut, TNextPara, TNextRes, TNextOut> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TNextOut"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextRes"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseFourWayPipe<Empty, TNextPara, TNextRes, TNextOut> Append<TNextPara, TNextRes, TNextOut>(this IPipeAppender pipe,
            BaseFourWayPipe<Empty, TNextPara, TNextRes, TNextOut> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加发布消息管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TNextOut"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextRes"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<TOut, TNextPara, TNextRes, TNextOut>(this IPipeAppender<TOut> pipe, 
            BaseMsgPublisher<TOut> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }


        /// <summary>
        /// 绑定异常错误重试
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static BaseFourWayPipe<TIn, TPara, TResult, TOut> ErrorRetry<TIn, TPara, TResult, TOut>(this BaseFourWayPipe<TIn, TPara, TResult, TOut> pipe,FlowEventOption option)
        {
            pipe.SetErrorRetry(option);
            return pipe;
        }



    }
}