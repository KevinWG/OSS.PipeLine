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
using OSS.Pipeline.Mos;

namespace OSS.Pipeline
{
    /// <summary>
    /// 管道部分组成基类
    /// </summary>
    /// <typeparam name="TInContext"></typeparam>
    public abstract class BasePipePart<TInContext> : IPipe
    {
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


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipePart(PipeType pipeType)
        {
            PipeType = pipeType;
            PipeCode = GetType().Name;
        }

        #region 流体启动和异步处理逻辑

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> Start(TInContext context)
        {
            var res = await InterHandling(context);
            if (!res)
            {
                await Block(context);
            }

            return true;
        }

        #endregion


        #region 实际管道扩展处理方法

        /// <summary>
        ///  管道堵塞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task Block(TInContext context)
        {
            return Task.CompletedTask;
        }

        #endregion


        #region 内部扩散方法

        /// <summary>
        ///  管道处理实际业务流动方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task<bool> InterHandling(TInContext context);


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
}