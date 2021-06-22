#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-28
*       
*****************************************************************************/

#endregion


using OSS.Pipeline.Base;

namespace OSS.Pipeline.Interface
{
    /// <summary>
    ///  管道链接器
    /// </summary>
    /// <typeparam name="TOutContext"></typeparam>
    public interface IPipeAppender<TOutContext> : IPipe
    {
        internal void InterAppend(BaseInPipePart<TOutContext> nextPipe);
        
        internal void InterAppend(BaseInPipePart<Empty> nextPipe);
    }
}