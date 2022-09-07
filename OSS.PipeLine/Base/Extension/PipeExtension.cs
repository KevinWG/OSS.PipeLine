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
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipe<TOut, TNextOut> Append<TOut, TNextOut>(this IPipeAppender<TOut> pipe, IPipe<TOut, TNextOut> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TNextOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipe<Empty, TNextOut> Append<TNextOut>(this IPipeAppender pipe,
                                                              IPipe<Empty, TNextOut> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        
        /// <summary>
        /// 绑定异常错误重试
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IPipe<TIn, TOut> SetErrorRetry<TIn, TOut>(this IPipe<TIn, TOut> pipe, FlowEventOption option)
        {
            pipe.SetErrorRetry(option);
            return pipe;
        }
    }
}