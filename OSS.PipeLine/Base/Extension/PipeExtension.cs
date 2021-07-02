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

using System.Collections.Generic;
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
            this IPipeAppender<TOutContext> pipe, BaseFourWayPipe<TOutContext, TNextPara, TNextResult, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

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
        public static BaseFourWayPipe<Empty, TNextPara,TNextResult, TNextOutContext> Append<TOutContext, TNextPara, TNextResult, TNextOutContext>(
            this IPipeAppender<TOutContext> pipe, BaseFourWayPipe<Empty, TNextPara, TNextResult, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
        
    

        /// <summary>
        /// 追加单入类型管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<OutContext>(this IPipeAppender<OutContext> pipe, BaseOneWayPipe<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }



        #region 特定类型指定

        /// <summary>
        /// 追加分支管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<OutContext>(this IPipeAppender<OutContext> pipe, BaseBranchGateway<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }

        /// <summary>
        /// 追加枚举器
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseMsgEnumerator<TMsgEnumerable, TMsg> Append<TMsgEnumerable, TMsg>(this IPipeAppender<TMsgEnumerable> pipe, BaseMsgEnumerator<TMsgEnumerable, TMsg> nextPipe)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        #endregion


    }
}