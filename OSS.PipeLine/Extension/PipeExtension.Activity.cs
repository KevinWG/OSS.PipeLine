using System;
using System.Threading.Tasks;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道扩展类
    /// </summary>
    public static partial class PipeExtension
    {
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseActivity AppendActivity<OutContext>(this IOutPipeAppender<OutContext> pipe, Func<Task<bool>> exeFunc, string pipeCode = null)
        {
            var nextPipe =new InterActivity(exeFunc,pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="OutContext"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseActivity<OutContext> AppendActivity<OutContext>(this IOutPipeAppender<OutContext> pipe, Func<OutContext, Task<bool>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterActivity<OutContext>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseEffectActivity< TResult> AppendEffectActivity< TResult>(this IOutPipeAppender<EmptyContext> pipe,
            Func<Task<(bool is_ok, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterEffectActivity< TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }
        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseEffectActivity<TFuncPara, TResult> AppendEffectActivity<TFuncPara, TResult>(this IOutPipeAppender<TFuncPara> pipe,
            Func<TFuncPara, Task<(bool is_ok, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }



        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseFuncActivity<TFuncPara, TResult> AppendFuncActivity<TFuncPara, TResult>(this IOutPipeAppender<TFuncPara> pipe,
            Func<TFuncPara, Task<(bool is_ok, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterFuncActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


        /// <summary>
        ///  追加活动管道
        /// </summary>
        /// <typeparam name="TFuncPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="exeFunc"></param>
        /// <param name="pipeCode"></param>
        /// <returns></returns>
        public static BaseFuncEffectActivity<TFuncPara, TResult> AppendFuncEffectActivity<TFuncPara, TResult>(this IOutPipeAppender<TFuncPara> pipe,
            Func<TFuncPara, Task<(bool is_ok, TResult result)>> exeFunc, string pipeCode = null)
        {
            var nextPipe = new InterFuncEffectActivity<TFuncPara, TResult>(exeFunc, pipeCode);
            pipe.InterAppend(nextPipe);
            return nextPipe;
        }


    }
}