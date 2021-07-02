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
using OSS.Pipeline.Activity.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TPassivePara类型参数，且此参数作为后续上下文传递给下一个节点，自身返回处理结果但无影响
    /// </summary>
    /// <typeparam name="TPassivePara"></typeparam>
    /// <typeparam name="TPassiveResult"></typeparam>
    public abstract class BasePassiveActivity<TPassivePara, TPassiveResult> :
        BaseThreeWayPassiveActivity<TPassivePara, TPassiveResult, TPassivePara> ,IPassiveActivity<TPassivePara, TPassiveResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BasePassiveActivity(string pipeCode = null) : base(pipeCode,PipeType.PassiveActivity)
        {
        }
        
        /// <inheritdoc />
        internal override async Task<TrafficResult<TPassiveResult, TPassivePara>> InterProcessPackage(TPassivePara context, string prePipeCode)
        {
            var tSignal = await Executing(context);
            return new TrafficResult<TPassiveResult, TPassivePara>(tSignal,
                tSignal.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, context);
        }

    }
}
