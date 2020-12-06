#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  异步消息延缓连接基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector
{
    /// <summary>
    /// 基础阻塞连接器
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseBlockConnector<InContext, OutContext> : BaseConnector<InContext, OutContext>,
        ISuspendTunnel<InContext>
        where InContext : FlowContext
        where OutContext : FlowContext
    {
        /// <inheritdoc />
        public abstract Task<bool> Push(InContext data);

        /// <summary>
        ///  需要调用此方法唤起接下来的操作
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task Pop(InContext data)
        {
            var outContext = Convert(data);
            return ToNextThrough(outContext);
        }

        internal override Task<bool> Through(InContext context)
        {
            return Push(context);
        }
    }

    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBlockConnector<TContext> : BaseBlockConnector<TContext, TContext>,
        ISuspendTunnel<TContext>
        where TContext : FlowContext
    {
        /// <inheritdoc />
        protected override TContext Convert(TContext inContextData)
        {
            return inContextData;
        }
    }
}
