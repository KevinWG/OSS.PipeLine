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
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Activity.Base
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TPassivePara类型参数，且此参数作为后续上下文传递给下一个节点，自身返回处理结果但无影响
    /// </summary>
    public abstract class BaseThreeWayPassiveActivity<THandlePara, THandleResult, TOut> : BaseThreeWayPassivePipe<THandlePara, THandleResult, TOut>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseThreeWayPassiveActivity(string pipeCode, PipeType pipeType) : base(pipeCode, pipeType)
        {
        }

        #region 外部业务扩展

        /// <summary>
        ///  具体执行扩展方法
        /// </summary>
        /// <param name="para">当前活动上下文信息</param>
        /// <returns>
        ///  -（活动是否处理成功，业务结果）
        /// traffic_signal：
        /// traffic_signal：     
        ///     Green_Pass  - 流体自动流入后续管道
        ///     Yellow_Wait - 管道流动暂停等待（仅当前处理业务），既不向后流动，也不触发Block。
        ///     Red_Block - 触发Block，业务流不再向后续管道传递。
        /// </returns>
        protected abstract Task<TrafficSignal<THandleResult>> Executing(THandlePara para);

        #endregion

    }
}
