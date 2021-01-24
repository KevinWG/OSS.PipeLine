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
using OSS.EventFlow.Connector.Interface;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector
{
    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseBufferConnector<InContext, OutContext> : BaseConnector<InContext, OutContext>,
        IBufferTunnel<InContext>
        where InContext : IFlowContext
        where OutContext : IFlowContext
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
    ///  异步缓冲连接器的默认实现
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public class DefaultBufferConnector<InContext, OutContext> : BaseBufferConnector<InContext, OutContext>
        where InContext : IFlowContext
        where OutContext : IFlowContext
    {
        private readonly IBufferConnectorProvider<InContext, OutContext> _provider;

        /// <inheritdoc/>
        public DefaultBufferConnector(IBufferConnectorProvider<InContext, OutContext> provider)
        {
            _provider = provider;
        }

        /// <inheritdoc/>
        public override Task<bool> Push(InContext data)
        {
            return _provider.Push(data);
        }

        /// <inheritdoc/>
        protected override OutContext Convert(InContext inContextData)
        {
            return _provider.Convert(inContextData);
        }
    }




    /// <summary>
    /// 异步消息延缓连接器
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBufferConnector<TContext> : BaseBufferConnector<TContext, TContext>,
        IBufferTunnel<TContext>
        where TContext : IFlowContext
    {
        /// <inheritdoc />
        protected override TContext Convert(TContext inContextData)
        {
            return inContextData;
        }
    }


    /// <inheritdoc />
    public class DefaultBufferConnector<TContext> : DefaultBufferConnector<TContext, TContext>
        where TContext : IFlowContext
    {
        /// <inheritdoc />
        public DefaultBufferConnector(IBufferConnectorProvider<TContext> provider) :base(provider)
        {
        }
    }

}
