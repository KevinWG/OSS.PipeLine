using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Flow.Interfaces;
using OSS.TaskFlow.Flow.Mos;

namespace OSS.TaskFlow.Flow
{
    /// <summary>
    /// 流运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlow<TDomain>
    {
        #region 存储处理

        public IFlowProvider<TDomain> MetaProvider { get; private set; }

        public void RegisteProvider(IFlowProvider<TDomain> metaPro)
        {
            MetaProvider = metaPro;
        }

        #endregion

        #region 内部扩展方法

        internal Task<ResultIdMo> GenerateRunId(FlowContext context)
        {
            return MetaProvider.GenerateRunId(context);
        }

        #endregion

        //  获取流核心数据信息
        //public abstract Task GetFlowInfo(TaskReq<> req);


    }
}
