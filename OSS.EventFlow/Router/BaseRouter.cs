using System;
using System.Threading.Tasks;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Router
{
    public abstract class BaseRouter
    {
        public RouterType RouterType { get; internal set; }
        public IBaseNode WorkNode { get;internal set; }
        
        protected BaseRouter()
        {
            RouterType = RouterType.Serial;
        }
        
        public virtual Task MoveIn(IExecuteData preData, BaseRouter preNode)
        {
            return Task.CompletedTask;
        }

        internal abstract Task MoveNext(IExecuteData preData);
    }

    public static class FlowrouterExtention
    {
        /// <summary>
        ///  设置顺序节点
        /// </summary>
        /// <param name="flowRouter"></param>
        /// <param name="node"></param>
        public static void WithSerial(this BaseRouter flowRouter, IBaseNode node)
        {
            flowRouter.RouterType = RouterType.Serial;
        }

        /// <summary>
        ///  设置循环节点
        /// </summary>
        /// <param name="flowRouter"></param>
        /// <param name="condition"></param>
        /// <param name="next"></param>
        public static void WithCircle(this BaseRouter flowRouter, Func<IExecuteData, Task<bool>> condition,
          params  IBaseNode[] next)
        {
            flowRouter.RouterType = RouterType.Cycle;
        }

        public static void WithCircle(this BaseRouter flowRouter, Func<IExecuteData,Task<bool>> condition,Func<IExecuteData,Task<IBaseNode[]>> next)
        {
            flowRouter.RouterType = RouterType.Cycle;
        }

        public static void WithBranch(this BaseRouter flowRouter, IBaseNode[] next)
        {
            flowRouter.RouterType = RouterType.Branch;

        }
        public static void WithBranch(this BaseRouter flowRouter, Func<IExecuteData, Task<IBaseNode[]>> next)
        {
            flowRouter.RouterType = RouterType.Branch;

        }
    }


    public enum RouterType
    {
        Serial,
        Cycle,
        Branch
    }
}
