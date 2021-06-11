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
    public class SimpleFuncActivity<TFuncPara, TResult> : BaseFuncActivity<TFuncPara, TResult>
    {
        private readonly Func<TFuncPara, Task<(TrafficSingleValue tsValue, TResult result)>> _exeFunc;

        /// <inheritdoc />
        public SimpleFuncActivity(Func<TFuncPara, Task<(TrafficSingleValue tsValue, TResult result)>> exeFunc,string pipeCode = null)
        {
            if (!string.IsNullOrEmpty(pipeCode))
            {
                PipeCode = pipeCode;
            }
            _exeFunc = exeFunc ?? throw new ArgumentNullException(nameof(exeFunc), "执行方法不能为空!");
        }


        /// <inheritdoc />
        protected override Task<(TrafficSingleValue tsValue, TResult result)> Executing(TFuncPara contextData)
        {
            return _exeFunc(contextData);
        }
    }
}
