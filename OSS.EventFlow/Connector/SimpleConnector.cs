#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  简单连接基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Connector
{
 
    public  class SimpleConnector<TContext> : BaseConnector<TContext, TContext>
        where TContext : FlowContext
    {
        protected override TContext Convert(TContext inContextData)
        {
            return inContextData;
        }
    }
}
