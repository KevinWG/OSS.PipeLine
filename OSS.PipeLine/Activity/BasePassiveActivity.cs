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


using OSS.Pipeline.Activity.Base;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TPassivePara类型参数，且此参数作为后续上下文传递给下一个节点，自身返回处理结果但无影响
    /// </summary>
    /// <typeparam name="TPassivePara"></typeparam>
    /// <typeparam name="TPassiveRes"></typeparam>
    public abstract class BasePassiveActivity<TPassivePara, TPassiveRes> :
        BaseThreeWayPassiveActivity<TPassivePara, TPassiveRes, TPassivePara> //,IPassiveActivity<TPassivePara, TPassiveRes>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BasePassiveActivity(string pipeCode = null) : base(pipeCode,PipeType.PassiveActivity)
        {
        }
        
        /// <inheritdoc />
        internal override async Task<TrafficSignal<TPassiveRes, TPassivePara>> InterProcessPackage(TPassivePara context)
        {
            var tSignal = await Executing(context);
            return new TrafficSignal<TPassiveRes, TPassivePara>(tSignal.signal, tSignal.result, context, tSignal.msg);
        }
    }
}
