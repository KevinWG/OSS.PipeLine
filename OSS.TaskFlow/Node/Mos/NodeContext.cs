using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Flow.Mos;
using OSS.TaskFlow.Node.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node.Mos
{
    public class NodeContext : FlowContext
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

        public static async Task<ResultMo> CheckNodeContext(this NodeContext context, InstanceType insType,
            Func<Task<ResultIdMo>> idGenerater)
        {
            var res = insType == InstanceType.Stand
                ? new ResultMo()
                : await context.CheckFlowContext(insType, idGenerater);

            if (!res.IsSysOk())
                return res;
            
            if (string.IsNullOrEmpty(context.node_meta?.node_key))
            {
                res.sys_ret = (int) SysResultTypes.ConfigError;
                res.ret = (int) ResultTypes.InnerError;
                res.msg = "node metainfo has error!";

                return res;
            }

            if (!string.IsNullOrEmpty(context.run_id))
                return res;

            var idRes = await idGenerater.Invoke();
            if (!idRes.IsSuccess())
                return idRes;

            context.run_id = idRes.id;
            return res;
        }
    }

}
