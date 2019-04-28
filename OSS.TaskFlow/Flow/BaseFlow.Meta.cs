using OSS.TaskFlow.Flow.Interfaces;

namespace OSS.TaskFlow.Flow
{
    /// <summary>
    /// 流运行时元数据信息
    /// </summary>
    public abstract partial class BaseFlow<TFlowData>
    {
        #region 存储处理

        public IFlowProvider<TFlowData> MetaProvider { get; private set; }

        public void RegisteProvider(IFlowProvider<TFlowData> metaPro)
        {
            MetaProvider = metaPro;
        }

        #endregion


        //  获取流核心数据信息
        //public abstract Task GetFlowInfo(TaskReq<> req);


    }
}
