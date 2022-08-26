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

namespace OSS.Pipeline
{
    public class WatchResult: TrafficSignal
    {
        public WatchResult(SignalFlag signal, object activityResult,  string msg):base(signal,msg)
        {
            activity_result     = activityResult;
        }

        /// <summary>
        ///  活动（activity）的执行结果
        /// </summary>
        public object activity_result { get; }
    }
    internal static class WatchResultMap
    {
        public static WatchResult ToWatchResult<TActivityResult>(this TrafficSignal<TActivityResult> tRes)
        {
            return new WatchResult(tRes.signal, tRes.result,  tRes.msg);
        }

    }
}