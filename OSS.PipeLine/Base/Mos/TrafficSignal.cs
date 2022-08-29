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


namespace OSS.Pipeline
{
    /// <inheritdoc />
    public class TrafficSignal<TRes, TOut> : TrafficSignal<TRes>
    {
        /// <summary>
        ///    输出对象（ 下节管道的输入 ）
        /// </summary>
        public TOut output { get; }

        /// <inheritdoc />
        public TrafficSignal(TRes res, TOut output) : base(res)
        {
            this.output = output;
        }

        /// <inheritdoc />
        public TrafficSignal(SignalFlag signalFlag, TRes res,  string trafficMsg = null) : this(signalFlag, res,default, trafficMsg)
        {
        }

        /// <inheritdoc />
        public TrafficSignal(SignalFlag signalFlag, TRes res,TOut output, string trafficMsg = null) : base(signalFlag,res, trafficMsg)
        {
            this.output = output;
        }
    }

    /// <summary>
    ///  流动信号
    /// </summary>
    public class TrafficSignal<TRes>: TrafficSignal
    {
        /// <summary>
        /// 流动通行信号（绿色通行）- 附带结果
        /// </summary>
        /// <param name="res"> 返回结果 </param>
        public TrafficSignal(TRes res) : this(SignalFlag.Green_Pass, res, string.Empty)
        {
        }
        
        /// <summary>
        /// 流动信号
        /// </summary>
        /// <param name="signalFlag"></param>
        /// <param name="trafficMsg"></param>
        /// <param name="res"></param>
        public TrafficSignal(SignalFlag signalFlag, TRes res, string trafficMsg = null):base(signalFlag, trafficMsg)
        {
            result = res;
        }
        
        /// <summary>
        ///结果
        /// </summary>
        public TRes result { get; }
    }
    
    /// <summary>
    ///  流动信号
    /// </summary>
    public class TrafficSignal
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
}
