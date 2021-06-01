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

namespace OSS.PipeLine.Interface
{
    /// <summary>
    ///  管道链接器
    /// </summary>
    /// <typeparam name="TOutContext"></typeparam>
    internal interface IPipeAppender<TOutContext> : IPipe
    {
        void InterAppend<NextOutContext>(BasePipe<TOutContext, NextOutContext> nextPipe);

        ///// <summary>
        ///// 追加管道
        ///// </summary>
        ///// <param name="nextPipe"></param>
        ///// <typeparam name="NextOutContext"></typeparam>
        ///// <returns>下个管道的追加器</returns>
        //BasePipe<TOutContext, NextOutContext> Append<NextOutContext>(BasePipe<TOutContext, NextOutContext> nextPipe);

        ///// <summary>
        /////   添加网关
        ///// </summary>
        ///// <param name="nextPipe"></param>
        //BaseBranchGateway<TOutContext> Append(BaseBranchGateway<TOutContext> nextPipe);
    }

}