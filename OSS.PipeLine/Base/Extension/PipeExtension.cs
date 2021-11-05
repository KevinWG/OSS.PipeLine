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
        /// <typeparam name="TOutContext"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseFourWayPipe<TOutContext, TNextPara, TNextResult, TNextOutContext> Append<TOutContext, TNextPara, TNextResult, TNextOutContext>(
            this IPipeAppender<TOutContext> pipe, 
            BaseFourWayPipe<TOutContext, TNextPara, TNextResult, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOutContext> Append<TNextPara, TNextResult, TNextOutContext>(this IPipeAppender pipe,
            BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加发布消息管道
        /// </summary>
        /// <typeparam name="TOutContext"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<TOutContext, TNextPara, TNextResult, TNextOutContext>(this IPipeAppender<TOutContext> pipe, 
            BaseMsgPublisher<TOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }
    }
}