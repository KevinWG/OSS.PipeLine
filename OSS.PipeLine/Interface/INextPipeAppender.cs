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

using OSS.EventFlow.Gateway;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    ///  管道链接器
    /// </summary>
    /// <typeparam name="TNextInContext"></typeparam>
    public interface IPipeAppender<TNextInContext>:IPipe
        //where TNextInContext : IPipeContext
    {
        /// <summary>
        /// 追加管道
        /// </summary>
        /// <param name="nextPipe"></param>
        /// <typeparam name="NextOutContext"></typeparam>
        /// <returns>下个管道的追加器</returns>
        BaseSinglePipe<TNextInContext, NextOutContext> Append<NextOutContext>(
            BaseSinglePipe<TNextInContext, NextOutContext> nextPipe);
            //where NextOutContext : IPipeContext;

        /// <summary>
        ///   添加网关
        /// </summary>
        /// <param name="nextPipe"></param>
        BaseBranchGateway<TNextInContext> Append(BaseBranchGateway<TNextInContext> nextPipe);
    }

}