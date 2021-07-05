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
    /// <typeparam name="TMsgEnumerable">消息的枚举类型如 IList&lt;TMsg&gt;</typeparam>
    public class BaseMsgEnumerator<TMsgEnumerable, TMsg> : BaseThreeWayPipe<TMsgEnumerable, Empty, TMsgEnumerable>
        where TMsgEnumerable : IEnumerable<TMsg>
    {
        /// <summary>
        /// 消息转化基类 
        /// </summary>
        public BaseMsgEnumerator(string pipeCode = null) : base(pipeCode, PipeType.MsgEnumerator)
        {
        }

        /// <summary>
        ///  过滤处理消息
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TMsg> FilterMsg(TMsgEnumerable msgs)
        {
            return msgs;
        }


        /// <inheritdoc />
        internal override async Task<TrafficResult<Empty, TMsgEnumerable>> InterProcessPackage(TMsgEnumerable msgs,
            string prePipeCode)
        {
            if (_iterator == null)
            {
                throw new NullReferenceException($"{GetType().Name}枚举器的迭代执行程序未赋值!");
            }

            var parallelPipes = msgs.Select(m => _iterator.InterPreCall(m, PipeCode));

            var res = (await Task.WhenAll(parallelPipes)).Any(r => r.signal == SignalFlag.Green_Pass)
                ? new TrafficResult<Empty, TMsgEnumerable>(SignalFlag.Green_Pass, string.Empty, string.Empty,
                    Empty.Default, msgs)
                : new TrafficResult<Empty, TMsgEnumerable>(SignalFlag.Red_Block, PipeCode, "所有分支运行失败！", Empty.Default,
                    msgs);

            return res;

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
            {
                throw new NullReferenceException($"{GetType().Name}枚举器的迭代执行程序未赋值!");
            }

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
                pipe_type = _iterator.PipeType
            };
            return pipe;
        }

        #endregion

    }
}
