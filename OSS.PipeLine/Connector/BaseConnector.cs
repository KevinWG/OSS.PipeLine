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

using OSS.PipeLine.Mos;
using System.Threading.Tasks;

namespace OSS.PipeLine.Connector
{
    /// <summary>
    /// 连接基类
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseConnector<InContext, OutContext> : BaseSinglePipe<InContext, OutContext>
    {
        /// <summary>
        /// 连接基类构造函数
        /// </summary>
        protected BaseConnector() : base(PipeType.Connector)
        {
        }

        protected BaseConnector(PipeType type) : base(type)
        {

        }

        /// <summary>
        ///  连接消息体的转换功能
        ///     如果是异步消息缓冲连接，会在唤起时执行此方法
        /// </summary>
        /// <param name="inContextData"></param>
        /// <returns></returns>
        protected abstract OutContext Convert(InContext inContextData);

        internal override async Task<bool> InterHandling(InContext context)
        {
            var outContext = Convert(context);
            await ToNextThrough(outContext);
            return true;
        }
    }
}
