using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventNode.Interfaces;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode<TTContext, TTRes>
    {
        public InstanceType InstanceType { get; internal set; }
        public FollowType RunType { get; internal set; }

        protected BaseNode()
        {
            InstanceType = InstanceType.Stand;
            RunType = FollowType.WithNode;
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
