using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.TaskFlow.Node.Interfaces;
using OSS.TaskFlow.Node.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Node
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseStandNode<TReq, TRes>
    {
        protected BaseStandNode()
        {
            InstanceType = InstanceType.Stand;
        }
        
        #region 存储处理

        public IStandNodeProvider<TReq> MetaProvider => (IStandNodeProvider<TReq>)m_metaProvider;

        public void RegisteProvider(IStandNodeProvider<TReq> metaPro)
        {
            base.RegisteProvider_Internal(metaPro);
        }
        
        #endregion

        #region 重写基类方法

        internal async Task<TRes> InitailRunId(NodeContext context)
        {
            if (!string.IsNullOrEmpty(context.run_id))
                return new TRes();

            var idRes=await MetaProvider.GenerateRunId(context);
            if (idRes.IsSuccess())
            {
                context.run_id = idRes.id;
            }

            return idRes.ConvertToResultInherit<TRes>();
        }

        #endregion
    }

}
