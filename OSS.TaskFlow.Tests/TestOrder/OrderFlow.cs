using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.FlowLine;
using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.Tests.TestOrder
{
    public class OrderFlow : BaseFlow<OrderInfo>
    {
        public override Task<ResultListMo<NodeMeta>> GetTaskMetas(ExcuteReq context)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ResultMo<OrderInfo>> Apply()
        {
            throw new System.NotImplementedException();
        }

        public override Task<ResultMo> Entry(ExcuteReq req)
        {
            throw new System.NotImplementedException();
        }

        public override Task End()
        {
            throw new System.NotImplementedException();
        }
    }
}
