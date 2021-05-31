
#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  默认连接器提供者
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-01-24
*       
*****************************************************************************/

#endregion


using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Impls.Interface
{
    /// <summary>
    ///  默认连接器提供者
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public interface IConnectorProvider<in InContext, out OutContext>
        where InContext : IPipeContext
        where OutContext : IPipeContext
    {
        /// <summary>
        ///  连接消息体的转换功能
        ///     如果是异步消息缓冲连接，会在唤起时执行此方法
        /// </summary>
        /// <param name="inContextData"></param>
        /// <returns></returns>
        OutContext Convert(InContext inContextData);
    }
}