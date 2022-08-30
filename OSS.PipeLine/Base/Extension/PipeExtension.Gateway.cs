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

using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        /// 追加分支管道
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<TOut>(this IPipeAppender<TOut> pipe, BaseBranchGateway<TOut> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }
        
    }
}