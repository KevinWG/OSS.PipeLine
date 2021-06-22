using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TOutContext"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BasePipe<TOutContext, TNextPara, TNextResult, TNextOutContext> Append<TOutContext, TNextPara, TNextResult, TNextOutContext>(
            this IPipeAppender<TOutContext> pipe, BasePipe<TOutContext, TNextPara, TNextResult, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        ///  追加普通管道
        /// </summary>
        /// <typeparam name="TOutContext"></typeparam>
        /// <typeparam name="TNextOutContext"></typeparam>
        /// <typeparam name="TNextPara"></typeparam>
        /// <typeparam name="TNextResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static BasePipe<Empty, TNextPara,TNextResult, TNextOutContext> Append<TOutContext, TNextPara, TNextResult, TNextOutContext>(
            this IPipeAppender<TOutContext> pipe, BasePipe<Empty, TNextPara, TNextResult, TNextOutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }

        /// <summary>
        /// 追加拦截类型管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static void Append<OutContext>(this IPipeAppender<OutContext> pipe, BaseInterceptPipe<OutContext> nextPipe)
        {
            pipe.InterAppend(nextPipe);
        }
        
    
    }
}