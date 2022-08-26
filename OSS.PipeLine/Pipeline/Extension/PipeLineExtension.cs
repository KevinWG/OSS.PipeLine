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


using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    /// EventFlow 创建工厂
    /// </summary>
    public static partial class PipelineExtension
    {
        ///// <summary>
        /////  追加下一个节点
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <typeparam name="TNextPara"></typeparam>
        ///// <typeparam name="TNextOut"></typeparam>
        ///// <typeparam name="TNextRes"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="nextPipe"></param>
        ///// <returns></returns>
        //public static IPipelineConnector<TIn, TNextOut> Then<TIn,  TOut,TNextPara, TNextRes, TNextOut>(this IPipelineConnector<TIn, TOut> pipe,
        //    BaseFourWayPipe<TOut, TNextPara, TNextRes, TNextOut> nextPipe)
        //{
        //    return pipe.Set(nextPipe);
        //}

        ///// <summary>
        /////  追加下一个节点
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <typeparam name="TNextPara"></typeparam>
        ///// <typeparam name="TNextOut"></typeparam>
        ///// <typeparam name="TNextRes"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="nextPipe"></param>
        ///// <returns></returns>
        //public static IPipelineConnector<TIn, TNextOut> Then<TIn, TOut, TNextPara, TNextRes, TNextOut>(this IPipelineConnector<TIn, TOut> pipe,
        //    BaseFourWayPipe<Empty, TNextPara, TNextRes, TNextOut> nextPipe)
        //{
        //    return pipe.Set(nextPipe);
        //}
        
        #region 生成Pipeline

        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <param name="startPipe"></param>
        /// <param name="endPipe"></param>
        /// <param name="flowPipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Pipeline<TIn, TEndOut> AsPipeline<TIn, TEndIn, TEndPara, TEndResult, TEndOut>(this BaseInPipePart<TIn> startPipe,
            BaseFourWayPipe<TEndIn, TEndPara, TEndResult, TEndOut> endPipe, string flowPipeCode, PipeLineOption option = null)
        {
            return new Pipeline<TIn, TEndOut>(flowPipeCode, startPipe, endPipe, option);
        }

        /// <summary>
        /// 根据首位两个管道建立流体
        /// </summary>
        /// <param name="startPipe"></param>
        /// <param name="endPipe"></param>
        /// <param name="flowPipeCode"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static EmptyEntryPipeline<TEndOut> AsPipeline<TEndIn, TEndPara, TEndResult, TEndOut>(this BaseInPipePart<Empty> startPipe,
            BaseFourWayPipe<TEndIn, TEndPara, TEndResult, TEndOut> endPipe,
            string flowPipeCode, PipeLineOption option = null)
        {
            return new EmptyEntryPipeline<TEndOut>(flowPipeCode, startPipe, endPipe, option);
        }

        ///// <summary>
        /////  根据当前连接信息创建Pipeline
        ///// </summary>
        ///// <typeparam name="TIn"></typeparam>
        ///// <typeparam name="TOut"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="pipeCode"></param>
        ///// <param name="option"></param>
        ///// <returns></returns>
        //public static Pipeline<TIn, TOut> AsPipeline<TIn, TOut>(this IPipelineConnector<TIn, TOut> pipe,
        //    string pipeCode, PipeLineOption option = null)
        //{
        //    var newPipe = new Pipeline<TIn, TOut>(pipeCode, pipe.StartPipe, pipe.EndAppender, option);
        //    pipe.StartPipe   = null;
        //    pipe.EndAppender = null;
        //    return newPipe;
        //}

        ///// <summary>
        /////  根据当前连接信息创建Pipeline
        ///// </summary>
        ///// <typeparam name="TOut"></typeparam>
        ///// <param name="pipe"></param>
        ///// <param name="pipeCode"></param>
        ///// <param name="option"></param>
        ///// <returns></returns>
        //public static EmptyEntryPipeline<TOut> AsPipeline< TOut>(this IPipelineConnector<Empty, TOut> pipe, string pipeCode, PipeLineOption option = null)
        //{
        //    var newPipe = new EmptyEntryPipeline<TOut>(pipeCode, pipe.StartPipe, pipe.EndAppender, option);
        //    pipe.StartPipe   = null;
        //    pipe.EndAppender = null;
        //    return newPipe;
        //}

        #endregion

    }
}