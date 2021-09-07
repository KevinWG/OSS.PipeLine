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
using OSS.Pipeline.Interface;

namespace OSS.Pipeline
{
    /// <summary>
    /// 消息转化基类
    /// </summary>
    /// <typeparam name="TMsg">消息具体类型</typeparam>
    public class MsgEnumerator<TMsg> : BaseThreeWayPipe<IEnumerable<TMsg>, Empty, IEnumerable<TMsg>>
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
        protected virtual IEnumerable<TMsg> FilterMsg(IEnumerable<TMsg> msgs)
        {
            return _msgFilter != null ? _msgFilter(msgs)  : msgs;
        }
        
        /// <inheritdoc />
        internal override async Task<TrafficResult<Empty, IEnumerable<TMsg>>> InterProcessPackage(IEnumerable<TMsg> msgs,
            string prePipeCode)
        {
            if (_iterator == null)
                throw new NullReferenceException($"{GetType().Name}枚举器的迭代执行程序未赋值!");

            var parallelPipes = msgs.Select(m => _iterator.InterPreCall(m, PipeCode));

            return (await Task.WhenAll(parallelPipes)).Any(r => r.signal == SignalFlag.Green_Pass)
                ? new TrafficResult<Empty, IEnumerable<TMsg>>(SignalFlag.Green_Pass, string.Empty, string.Empty, Empty.Default, msgs)
                : new TrafficResult<Empty, IEnumerable<TMsg>>(SignalFlag.Red_Block, PipeCode, "所有分支运行失败！", Empty.Default, msgs);
        }


        #region 管道处理

        private BaseInPipePart<TMsg> _iterator;
        internal void InterSetIterator(BaseInPipePart<TMsg> iterator)
        {
            _iterator = iterator;
        }

        #endregion

        #region 初始化

        /// <inheritdoc />
        internal override void InterInitialContainer(IPipeLine flowContainer)
        {
            if (_iterator == null)
                throw new NullReferenceException($"{GetType().Name}枚举器的迭代执行程序未赋值!");
            
            base.InterInitialContainer(flowContainer);
            _iterator.InterInitialContainer(flowContainer);
        }

        #endregion

        #region 路由

        internal override PipeRoute InterToRoute(bool isFlowSelf = false)
        {
            var pipe = base.InterToRoute(isFlowSelf);
            pipe.iterator = new PipeRoute()
            {
                pipe_code = _iterator.PipeCode,
                pipe_type = _iterator.PipeType,

                next = _iterator.InterToRoute(isFlowSelf)
            };
            return pipe;
        }

        #endregion

    }
}
