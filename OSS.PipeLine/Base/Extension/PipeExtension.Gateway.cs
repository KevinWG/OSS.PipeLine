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

using System;
using OSS.PipeLine.Gateway.Default;
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
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<OutContext>(this IPipeAppender<OutContext> pipe, BaseBranchGateway<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }
        
        // == 分支需要添加多个子节点，不提供简化方法

        ///// <summary>
        ///// 追加分支管道
        ///// </summary>
        ///// <typeparam name="TContext"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="pipeCode"></param>
        ///// <param name="_conditionFilter"></param>
        ///// <returns></returns>
        //public static void AppendBranch<TContext>(this IPipeAppender<TContext> pipe, string pipeCode="",
        //    Func<TContext, IPipeMeta, string, bool> _conditionFilter=null)
        //{
        //    pipe.InterAppend(new SimpleBranchGateway<TContext>(pipeCode, _conditionFilter));
        //}
        
    }
}