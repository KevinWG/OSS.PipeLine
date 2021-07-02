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
using System.Threading.Tasks;

namespace OSS.Pipeline
{

    /// <inheritdoc />
    public class SimplePassiveActivity<TPassivePara, TResult> : BasePassiveActivity<TPassivePara, TResult>
    {
        private readonly Func<TPassivePara, Task<TrafficSignal<TResult>>> _exePassive;

        /// <inheritdoc />
        public SimplePassiveActivity(string pipeCode,Func<TPassivePara, Task<TrafficSignal<TResult>>> exePassive):base(pipeCode)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exePassive = exePassive ?? throw new ArgumentNullException(nameof(exePassive), "执行方法不能为空!");
        }


        /// <inheritdoc />
        protected override Task<TrafficSignal<TResult>> Executing(TPassivePara contextData)
        {
            return _exePassive(contextData);
        }
    }
}
