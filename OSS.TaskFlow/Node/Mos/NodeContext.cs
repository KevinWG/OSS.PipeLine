using System;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.Node.Mos
{
    public class NodeContext:FlowContext
    {
        /// <summary>
        ///  当前流-节点元信息
        /// </summary>
        public NodeMeta node_meta { get; set; }
    }

    public static class NodeContextExtention
    {
        public static NodeContext ConvertToTaskContext(this FlowContext node)
        {
            var nodeCon = new NodeContext
            {
                run_id = node.run_id,
                flow_meta = node.flow_meta
            };
            return nodeCon;
        }


        public static ResultMo CheckNodeContext(this NodeContext context, Func<string> idGenerate = null)
        {
            var res= context.CheckFlowContext(idGenerate);
            if (!res.IsSysOk())
                return res;

            if (string.IsNullOrEmpty(context.node_meta?.node_key))
            {
                res.sys_ret = (int)SysResultTypes.ConfigError;
                res.ret = (int)ResultTypes.InnerError;
                res.msg = "node metainfo has error!";
            }
            return res;
        }
    }

}
