using System;
using System.Threading.Tasks;
using OSS.EventFlow.Mos;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Agent
{
    
    public static class EventAgentExtention
    {
        /// <summary>
        ///  设置顺序节点
        /// </summary>
        /// <param name="flowRouter"></param>
        /// <param name="node"></param>
        public static void WithSerial(this BaseAgent flowRouter, IEventNode node)
        {
            flowRouter.RouterType = RouterType.Serial;
        }

        /// <summary>
        ///  设置循环节点
        /// </summary>
        /// <param name="flowRouter"></param>
        /// <param name="condition"></param>
        /// <param name="next"></param>
        public static void WithCircle(this BaseAgent flowRouter, Func<IExecuteData, Task<bool>> condition,
            params IEventNode[] next)
        {
            flowRouter.RouterType = RouterType.Cycle;
        }

        public static void WithCircle(this BaseAgent flowRouter, Func<IExecuteData, Task<bool>> condition,
            Func<IExecuteData, Task<IEventNode[]>> next)
        {
            flowRouter.RouterType = RouterType.Cycle;
        }

        public static void WithBranch(this BaseAgent flowRouter, IEventNode[] next)
        {
            flowRouter.RouterType = RouterType.Branch;

        }

        public static void WithBranch(this BaseAgent flowRouter, Func<IExecuteData, Task<IEventNode[]>> next)
        {
            flowRouter.RouterType = RouterType.Branch;

        }
    }

}
