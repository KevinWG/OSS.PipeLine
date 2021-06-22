#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  通行信号
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System;

namespace OSS.Pipeline
{
    /// <summary>
    ///  流动信号
    /// </summary>
    public readonly struct TrafficSignal<TRes>
    {

        /// <summary>
        /// 流动通行信号（绿色通行）-附带结果
        /// </summary>
        /// <param name="res">结果数据</param>
        public TrafficSignal(TRes res) : this(SignalFlag.Green_Pass, res, string.Empty)
        {
        }

        /// <inheritdoc />
        public TrafficSignal(SignalFlag signalFlag, string trafficMsg) : this(signalFlag,default, trafficMsg)
        {
        }

        /// <summary>
        /// 流动信号
        /// </summary>
        /// <param name="signalFlag"></param>
        /// <param name="trafficMsg"></param>
        /// <param name="res"></param>
        public TrafficSignal(SignalFlag signalFlag, TRes res, string trafficMsg)
        {
            signal = signalFlag;
            msg    = trafficMsg;
            result = res;
        }

        /// <summary>
        ///  信号
        /// </summary>
        public SignalFlag signal { get; }

        /// <summary>
        ///  消息
        /// </summary>
        public string msg { get; }
        
        /// <summary>
        ///结果
        /// </summary>
        public TRes result { get; }
    }


    /// <summary>
    ///  流动信号
    /// </summary>
    public readonly struct TrafficSignal
    {
        /// <summary>
        /// 默认绿灯信号
        /// </summary>
        public static TrafficSignal GreenSignal { get; } =
            new TrafficSignal(SignalFlag.Green_Pass, string.Empty);

        /// <summary>
        /// 流动信号
        /// </summary>
        /// <param name="signalFlag"></param>
        /// <param name="trafficMsg"></param>
        public TrafficSignal(SignalFlag signalFlag, string trafficMsg)
        {
            signal = signalFlag;
            msg    = trafficMsg;
        }

        /// <summary>
        ///  信号
        /// </summary>
        public SignalFlag signal { get; }

        /// <summary>
        ///  消息
        /// </summary>
        public string msg { get; }
    }


    /// <summary>
    ///  通行信号
    /// </summary>
    public enum SignalFlag
    {
        /// <summary>
        ///  正常通过
        /// </summary>
        Green_Pass,

        /// <summary>
        ///  警告等待
        /// </summary>
        Yellow_Wait,

        /// <summary>
        /// 异常阻塞
        /// </summary>
        Red_Block
    }
}
