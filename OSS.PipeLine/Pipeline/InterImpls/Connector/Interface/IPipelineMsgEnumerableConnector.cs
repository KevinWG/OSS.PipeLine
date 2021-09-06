using System.Collections.Generic;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMsg">消息具体类型</typeparam>
    /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
    /// <typeparam name="TInContext"></typeparam>
    public interface IPipelineMsgEnumerableConnector<TInContext, TMsgEnumerable, TMsg>
        where TMsgEnumerable : IEnumerable<TMsg>
    {
        internal BaseInPipePart<TInContext>              StartPipe { get; set; }
        internal MsgEnumerator<TMsgEnumerable, TMsg> EndPipe   { get; set; }
    }
}