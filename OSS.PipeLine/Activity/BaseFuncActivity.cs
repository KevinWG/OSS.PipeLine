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


using OSS.Pipeline.Interface;
using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TFuncPara类型参数，且此参数作为后续上下文传递给下一个节点，自身返回处理结果但无影响
    /// </summary>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncActivity<TFuncPara, TFuncResult> :
        BaseFuncPipe<TFuncPara, TFuncPara> ,IFuncActivity<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncActivity() : base(PipeType.FuncActivity)
        {
        }

        #region 流体启动执行

        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<TFuncResult> Execute(TFuncPara para)
        {
            var (traffic_signal, result) = await Executing(para);
            await Watch(PipeCode, PipeType, WatchActionType.Executed, para, result);
            
            if (traffic_signal.signal == TrafficSignal.Red_Block)
            {
                await InterBlock(para,PipeCode);
            }
            else if (traffic_signal.signal == TrafficSignal.Green_Pass)
            {
                await ToNextThrough(para);
            }

            return result;
        }

        #endregion


        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        /// (bool traffic_signal,TResult result)-（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 暂停执行，既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<(TrafficSingleValue tsValue, TFuncResult result)> Executing(TFuncPara para);

      
    }
}
