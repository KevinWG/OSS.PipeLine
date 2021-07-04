#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道追加器接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using OSS.Pipeline.Base;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public interface IPipelineConnector<TInContext, TOutContext>
    {
        internal BaseInPipePart<TInContext> StartPipe   { get; set; }
        internal IPipeAppender<TOutContext> EndAppender { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    /// <typeparam name="TOutContext"></typeparam>
    public interface IPipelineBranchConnector<TInContext, TOutContext>
    {
        internal BaseInPipePart<TInContext>     StartPipe   { get; set; }
        internal BaseBranchGateway<TOutContext> EndBranchPipe { get; set; }
    }


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
        internal BaseMsgEnumerator<TMsgEnumerable, TMsg> EndPipe   { get; set; }
    }
}
