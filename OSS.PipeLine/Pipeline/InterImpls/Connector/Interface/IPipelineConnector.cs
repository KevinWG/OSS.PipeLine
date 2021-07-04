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

using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Pipeline.InterImpls.Connector
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
}
