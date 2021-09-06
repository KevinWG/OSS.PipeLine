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
    public static partial class MsgIteratorExtension
    {
        /// <summary>
        ///  设置枚举迭代器
        /// </summary>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TMsg">消息具体类型</typeparam>
        /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static IPipeAppender<TNextOutContext> SetIterator<TMsgEnumerable, TMsg, TNextPara, TNextResult, TNextOutContext>(this MsgEnumerator<TMsgEnumerable, TMsg> pipe, BaseFourWayPipe<TMsg, TNextPara, TNextResult, TNextOutContext> nextPipe)
            where TMsgEnumerable:IEnumerable<TMsg>
        {
            pipe.InterSetIterator(nextPipe);
            return nextPipe;
        }
        
        /// <summary>
        /// 设置枚举迭代器
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void SetIterator<TMsgEnumerable,TMsg>(this MsgEnumerator<TMsgEnumerable, TMsg> pipe, BaseOneWayPipe<TMsg> nextPipe)
            where TMsgEnumerable : IEnumerable<TMsg>
        {
            pipe.InterSetIterator(nextPipe);
        }


    }
}