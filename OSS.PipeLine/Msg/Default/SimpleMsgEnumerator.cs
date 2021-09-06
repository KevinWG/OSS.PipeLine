#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.PipeLine -  简单消息枚举器
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-7-5
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using OSS.Pipeline;

namespace OSS.PipeLine.Msg.Default
{
    /// <summary>
    ///  简单消息枚举器（继承至 IList
    /// </summary>
    /// <typeparam name="TMsg"></typeparam>
    public class SimpleMsgList<TMsg> : MsgEnumerator<IList<TMsg>, TMsg>
    {
        /// <summary>
        /// 简单消息枚举器（继承至 IList
        /// </summary>
        /// <param name="pipeCode"></param>
        /// <param name="msgFilter"></param>
        public SimpleMsgList(string pipeCode = null, Func<IList<TMsg>, IList<TMsg>> msgFilter = null) :
            base(pipeCode, msgFilter)
        {
        }
    }
}
