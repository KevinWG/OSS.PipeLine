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
    public class SimpleMsgEnumerator<TMsg> : BaseMsgEnumerator<IList<TMsg>, TMsg>
    {
        private readonly Func<IList<TMsg>, IEnumerable<TMsg>> _msgFilter;

        public SimpleMsgEnumerator(string pipeCode = null, Func<IList<TMsg>, IEnumerable<TMsg>> msgFilter = null) :
            base(pipeCode)
        {
            _msgFilter = msgFilter;
        }

        protected override IEnumerable<TMsg> FilterMsg(IList<TMsg> msgs)
        {
            return _msgFilter?.Invoke(msgs) ?? msgs;
        }
    }
}
