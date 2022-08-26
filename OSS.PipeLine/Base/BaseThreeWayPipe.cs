#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道执行基类（主动三向类型 ）
    ///   输入：上游传递的上下文
    ///   输出：主动结果输出， 下游上下文参数输出
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BaseThreeWayPipe<TIn, TRes, TOut> : BaseFourWayPipe<TIn, TIn, TRes, TOut>,
        IPipeExecutor<TIn, TRes>
    {
        /// <inheritdoc />
        protected BaseThreeWayPipe(string pipeCode, PipeType pipeType) : base(pipeCode, pipeType)
        {
        }

        #region 流体外部扩展

        /// <summary>
        /// 外部执行方法 - 启动入口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<TRes> Execute(TIn para)
        {
            return (await InterProcess(para)).result;
        }
        
        #endregion


        #region 流体内部业务处理

        /// <inheritdoc />
        internal override async Task<TrafficResult> InterPreCall(TIn context)
        {
            return await InterProcess(context);
        }

        #endregion
    }
}