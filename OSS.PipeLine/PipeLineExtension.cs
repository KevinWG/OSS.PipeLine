#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - Pipeline 扩展
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
    /// EventFlow 创建工厂
    /// </summary>
    public static class PipeLineExtension
    {
        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <param name="startPipe"></param>
        /// <param name="endPipeAppender"></param>
        /// <param name="flowPipeCode"></param>
        /// <returns></returns>
        public static PipeLine<InFlowContext, OutFlowContext> AsFlowStartAndEndWith<InFlowContext, OutFlowContext>(
            this BasePipePart<InFlowContext> startPipe, IPipeAppender<OutFlowContext> endPipeAppender,
            string flowPipeCode = null)
        {
            var code = flowPipeCode ?? string.Concat(startPipe.PipeCode, "-", endPipeAppender.PipeCode);

            return new PipeLine<InFlowContext, OutFlowContext>(startPipe, endPipeAppender) {PipeCode = code};
        }


    }
}