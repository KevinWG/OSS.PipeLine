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
using OSS.Pipeline.Base.Base;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class MsgIteratorExtension
    {
        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <typeparam name="TMsgEnumerable"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BaseMsgEnumerator<TMsgEnumerable, TMsg> SetIterator<TMsgEnumerable, TMsg, TNextPara, TNextResult, TNextOutContext>(
            this BaseMsgEnumerator<TMsgEnumerable, TMsg> pipe, BasePipe<TMsg, TNextPara, TNextResult, TNextOutContext> nextPipe)
        where TMsgEnumerable:IEnumerable<TMsg>
        {
            pipe.InterSetIterator(nextPipe);
            return pipe;
        }
    }
}