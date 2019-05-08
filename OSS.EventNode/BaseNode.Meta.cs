using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode.Interfaces;
using OSS.EventNode.MetaMos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{

    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode<TTReq, TTRes> : BaseMetaProvider<NodeMeta>, IBaseNode<TTReq, TTRes>
        where TTReq : ExcuteReq
        where TTRes : ResultMo, new()
    {
        private const string _moduleName = "OSS.EventTask";

        public NodeMeta NodeMeta => GetConfig();
        public InstanceType InstanceNodeType { get; internal set; }

     
        protected BaseNode(NodeMeta meta) : base(meta)
        {
            InstanceNodeType = InstanceType.Stand;
        }
        
        #region 注册存储接口

        internal INodeProvider<TTReq> m_metaProvider;

        internal void RegisteProvider_Internal(INodeProvider<TTReq> metaPpro)
        {
            m_metaProvider = metaPpro;
        }

        #endregion
        
        #region 内部基础方法
        internal Task<IList<IBaseTask<TTReq>>> GetTaskMetas()
        {
            return m_metaProvider.GetTaskMetas();
        }
        #endregion
        
    }





}
