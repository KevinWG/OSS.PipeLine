#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  连接基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    /// 消息转化基类
    /// </summary>
    /// <typeparam name="TMsg">消息具体类型</typeparam>
    public class MsgEnumerator<TMsg> : BaseThreeWayPipe<IEnumerable<TMsg>, Empty, TMsg>
    {
        private readonly Func<IEnumerable<TMsg>, IEnumerable<TMsg>> _msgFilter = null;
        /// <summary>
        /// 消息转化基类 
        /// </summary>
        public MsgEnumerator(string pipeCode = null, Func<IEnumerable<TMsg>, IEnumerable<TMsg>>  msgFilter=null) : base(pipeCode, PipeType.MsgEnumerator)
        {
            _msgFilter = msgFilter;
        }
        
        /// <summary>
        ///  过滤处理消息
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TMsg> FilterMsgs(IEnumerable<TMsg> msgs)
        {
            return _msgFilter != null ? _msgFilter(msgs)  : msgs;
        }

        #region 管道内部业务处理
        
        /// <inheritdoc />
        internal override async Task<TrafficResult<Empty, TMsg>> InterProcessHandling(IEnumerable<TMsg> msgs, string prePipeCode)
        {
            var filterMsgs = FilterMsgs(msgs);
            if (filterMsgs==null)
                throw new ArgumentNullException(nameof(msgs), "消息枚举器列表数据不能为空!");
            
            var trafficRes = await InterWatchProcessPackage(filterMsgs, prePipeCode);

            switch (trafficRes.signal)
            {
                case SignalFlag.Green_Pass:
                {
                    return new TrafficResult<Empty, TMsg>(SignalFlag.Green_Pass, string.Empty, string.Empty, trafficRes.result, trafficRes.output_paras);
                }
                case SignalFlag.Red_Block:
                    await InterWatchBlock(filterMsgs, trafficRes);
                    break;
            }
            return trafficRes;
        }

        /// <inheritdoc />
        internal override async Task<TrafficResult<Empty, TMsg>> InterProcessPackage(IEnumerable<TMsg> msgs, string prePipeCode)
        {
            var parallelTasks = msgs.Select(ToNextThrough);

            return (await Task.WhenAll(parallelTasks)).Any(r => r.signal == SignalFlag.Green_Pass)
                ? new TrafficResult<Empty, TMsg>(SignalFlag.Green_Pass, string.Empty, string.Empty, Empty.Default, default)
                : new TrafficResult<Empty, TMsg>(SignalFlag.Red_Block, PipeCode, "所有分支运行失败！", Empty.Default, default);
        }


        #endregion
    }
}
