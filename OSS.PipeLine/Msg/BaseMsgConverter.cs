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


using System.Threading.Tasks;
using OSS.Pipeline.Base;

namespace OSS.Pipeline
{
    /// <summary>
    /// 消息转化基类
    /// </summary>
    /// <typeparam name="TInMsg"></typeparam>
    /// <typeparam name="TOutMsg"></typeparam>
    public abstract class BaseMsgConverter<TInMsg, TOutMsg> : BaseStraightPipe<TInMsg, TOutMsg>
    {
        /// <summary>
        /// 消息转化基类
        /// </summary>
        protected BaseMsgConverter() : base(PipeType.MsgConverter)
        {
        }

        /// <summary>
        ///  连接消息体的转换功能
        /// </summary>
        /// <param name="inContextData"></param>
        /// <returns></returns>
        protected abstract TOutMsg Convert(TInMsg inContextData);

        internal override Task<TrafficResult<TOutMsg, TOutMsg>> InterHandlePack(TInMsg context)
        {
            var outContext = Convert(context);
            return Task.FromResult(new TrafficResult<TOutMsg, TOutMsg>(SignalFlag.Green_Pass, string.Empty,
                string.Empty, outContext,outContext));
        }
    }
}
