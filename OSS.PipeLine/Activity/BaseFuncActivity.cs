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


using System;
using OSS.Pipeline.Interface;
using System.Threading.Tasks;
using OSS.Pipeline.Activity.Base;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TFuncPara类型参数，且此参数作为后续上下文传递给下一个节点，自身返回处理结果但无影响
    /// </summary>
    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public abstract class BaseFuncActivity<TFuncPara, TFuncResult> :
        BaseThreeWayFuncActivity<TFuncPara, TFuncResult, TFuncPara> ,IFuncActivity<TFuncPara, TFuncResult>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseFuncActivity() : base(PipeType.FuncActivity)
        {
        }
        
      

        /// <inheritdoc />
        internal override async Task<TrafficResult<TFuncResult, TFuncPara>> InterProcessPackage(TFuncPara context)
        {
            var tSignal = await Executing(context);
            return new TrafficResult<TFuncResult, TFuncPara>(tSignal,
                tSignal.signal == SignalFlag.Red_Block ? PipeCode : string.Empty, context);
        }

    }
}
