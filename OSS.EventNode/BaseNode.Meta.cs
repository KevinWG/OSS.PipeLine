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
        // 内部成员
        private const string _moduleName = "OSS.EventNode";
        //internal INodeProvider<TTReq> m_metaProvider;

        /// <summary>
        /// 节点mata 信息
        /// </summary>
        public NodeMeta NodeMeta => GetConfig();
        /// <summary>
        /// 节点实例类型
        /// </summary>
        public InstanceType InstanceNodeType { get; internal set; }


        protected BaseNode(NodeMeta meta) : base(meta)
        {
            ModuleName = _moduleName;
            InstanceNodeType = InstanceType.Stand;
        }
        
        #region 内部基础方法

        protected abstract Task<IList<IBaseTask<TTReq>>> GetTasks();

        #endregion

    }





}
