using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Node.Interfaces;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks;
using OSS.TaskFlow.Tasks.MetaMos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode
    {
        public InstanceType InstanceType { get; internal set; }

        public FlowNodeType FlowNodeType { get; internal set; }


        protected BaseNode()
        {
            FlowNodeType = FlowNodeType.None;
            InstanceType = InstanceType.Stand;
        }
        

        #region 注册存储接口

        internal INodeProvider m_metaProvider;

        internal void RegisteProvider_Internal(INodeProvider metaPpro)
        {
            m_metaProvider = metaPpro;
        }

        #endregion


        #region 内部扩展方法

        internal virtual Task<ResultIdMo> GenerateRunId(NodeContext context)
        {
            return Task.FromResult(new ResultIdMo(SysResultTypes.ConfigError, ResultTypes.ObjectStateError, "Node with data cann't generate run_id by itself!"));
        }

        #endregion

        #region 内部基础方法
        internal Task<Dictionary<TaskMeta, BaseTask>> GetTaskMetas(NodeContext context)
        {
            return m_metaProvider.GetTaskMetas(context);
        }
        #endregion



    }

}
