using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.FlowLine;
using OSS.TaskFlow.FlowLine.Mos;

namespace OSS.TaskFlow.Tests.TestOrder
{
    public class OrderFlow : BaseFlow
    {
        public override Task<ResultMo<FlowInfo>> Apply()
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
