#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体上下文数据实体接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-12-25
*       
*****************************************************************************/

#endregion

namespace OSS.EventFlow.Mos
{
    public interface IFlowContext
    {

    }

    /// <inheritdoc />
    public abstract class FlowContext<IdType>: IFlowContext
    {
        public IdType id
        {
            get;
            set;
        }
    }

}
