using System.Threading.Tasks;
using OSS.Common.ComModels;

namespace OSS.TaskFlow.FlowLine
{
    
    public abstract partial class BaseFlow<TFlowEntity> 
    {
        //public override Task<ResultMo<FlowInfo>> Apply()
        //{
        //    //  需创建数据
        //   return Task.FromResult(new ResultMo<FlowInfo>());
        //}
        public abstract Task<ResultMo<TFlowEntity>> Apply();

        public abstract Task<ResultMo> Entry(ExcuteReq req);

        public abstract Task End();


        //  获取流核心数据信息
        //public abstract Task GetFlowInfo(TaskReq<> req);
    }

}
