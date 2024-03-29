﻿#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using OSS.Pipeline.Base;
using System.Threading.Tasks;

namespace OSS.Pipeline
{
    /// <summary>
    ///  被动触发执行活动组件基类
    ///      传入TPassivePara类型参数，自身返回处理结果，且结果作为上下文传递给下一个节点
    /// </summary>
    /// <typeparam name="TPara"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public abstract class BasePassiveEffectActivity<TPara, TRes> : BaseThreeWayPassivePipe<TPara, TRes, TRes> //, IPassiveEffectActivity<TPassivePara, TPassiveRes>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BasePassiveEffectActivity(string pipeCode = null) : base(pipeCode, PipeType.EffectActivity | PipeType.EffectActivity)
        {
        }


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
        protected abstract Task<TrafficSignal<TRes>> Executing(TPara para);



        internal override async Task<TrafficSignal<TRes, TRes>> InterProcessing(TPara req)
        {
            var tSignal = await Executing(req);
            return new TrafficSignal<TRes, TRes>(tSignal.signal,tSignal.result, tSignal.result, tSignal.msg);
        }
    }
}