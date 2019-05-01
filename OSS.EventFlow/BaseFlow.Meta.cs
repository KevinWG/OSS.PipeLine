using OSS.EventFlow.Interfaces;

namespace OSS.EventFlow
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


        #endregion
        
    }
}
