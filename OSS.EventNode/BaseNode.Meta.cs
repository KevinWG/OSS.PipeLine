using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode.Interfaces;
using OSS.EventNode.MetaMos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{


    public abstract  class BaseMetaNode<TTContext, TTRes>:BaseMetaProvider<NodeMeta, BaseMetaNode<TTContext, TTRes>>
    {
        private const string _moduleName = "OSS.EventTask";
        public InstanceType InstanceType { get; internal set; }
        public OwnerType OwnerType { get; internal set; }
        
        protected BaseMetaNode():this(null)
        {
        }

        protected BaseMetaNode(NodeMeta meta):base(meta)
        {
            InstanceType = InstanceType.Stand;
            OwnerType = OwnerType.Node;
        }
    }

    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode<TTContext, TTRes>
    {
        protected BaseNode() : this(null)
        {
        }

        protected BaseNode(NodeMeta meta) : base(meta)
        {
        }


        #region 注册存储接口

        internal INodeProvider m_metaProvider;

        internal void RegisteProvider_Internal(INodeProvider metaPpro)
        {
            m_metaProvider = metaPpro;
        }

        #endregion
        
        #region 内部基础方法
        internal Task<Dictionary<TaskMeta, IBaseTask>> GetTaskMetas(TTContext context)
        {
            return m_metaProvider.GetTaskMetas(context);
        }
        #endregion
        
    }

}
