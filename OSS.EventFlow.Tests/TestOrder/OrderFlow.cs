using System.Threading.Tasks;
using OSS.EventFlow.FlowLine;

namespace OSS.EventFlow.Tests.TestOrder
{
    public class OrderFlow : BaseFlow<OrderInfo>
    {
        public override Task Entry()
        {
            throw new System.NotImplementedException();
        }
    }
}
