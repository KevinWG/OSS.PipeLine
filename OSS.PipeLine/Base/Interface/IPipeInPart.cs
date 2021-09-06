#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-28
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{

    internal interface IPipeRoute : IPipeMeta
    {
        /// <summary>
        ///  内部处理流的路由信息
        /// </summary>
        /// <returns></returns>
        internal PipeRoute InterToRoute(bool isFlowSelf = false);

        /// <summary>
        ///  内部处理流容器初始化赋值
        /// </summary>
        /// <param name="containerFlow"></param>
        internal abstract void InterInitialContainer(IPipeLine containerFlow);
    }

    /// <summary>
    ///   管道入口
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    internal interface IPipeInPart<in TInContext> : IPipeRoute
    {
        /// <summary>
        ///  内部管道 -- 唤起
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prePipeCode"></param>
        /// <returns></returns>
        internal  Task<TrafficResult> InterPreCall(TInContext context, string prePipeCode);

        /// <summary>
        /// 被连接到时执行
        /// </summary>
        /// <param name="prePipe">被追加的上级管道</param>
        internal void InterAppendTo(IPipeMeta prePipe);
    }
}