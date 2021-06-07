#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道部分组成基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Pipeline.Interface;
using OSS.Pipeline.InterImpls.Watcher;

namespace OSS.Pipeline.Base
{
    /// <summary>
    /// 管道组成基类
    /// </summary>
    public abstract class BasePipePart : IPipe
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipePart(PipeType pipeType)
        {
            PipeType = pipeType;
            PipeCode = GetType().Name;
        }

        /// <summary>
        ///  管道类型
        /// </summary>
        public PipeType PipeType { get; internal set; }

        /// <summary>
        ///  管道编码
        ///  默认等于  this.GetType().Name
        /// </summary>
        public string PipeCode { get; set; }

        /// <summary>
        ///  流容器
        /// </summary>
        protected IPipeLine LineContainer { get; set; }


        #region 管道监控

        internal PipeWatcherProxy WatchProxy { get; set; }

        internal Task<bool> Watch(WatchDataItem item)
        {
            if (WatchProxy != null)
            {
                return WatchProxy.Watch(item);
            }

            return InterUtil.FalseTask;
        }

        #endregion


        #region 内部扩散方法

        /// <summary>
        ///  内部处理流容器初始化赋值
        /// </summary>
        /// <param name="containerFlow"></param>
        internal abstract void InterInitialContainer(IPipeLine containerFlow);

        /// <summary>
        ///  内部处理流的路由信息
        /// </summary>
        /// <returns></returns>
        internal abstract PipeRoute InterToRoute(bool isFlowSelf = false);

        #endregion
    }

    /// <summary>
    /// 管道进口基类
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public abstract class BaseInPipePart<TInContext> : BasePipePart
    {
        /// <inheritdoc />
        protected BaseInPipePart(PipeType pipeType) : base(pipeType)
        {
        }

  

        #region 管道的业务处理

        /// <summary>
        ///  管道处理实际业务流动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<bool> InterStart(TInContext context);

        #endregion
    }

}