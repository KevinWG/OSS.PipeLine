using System.Threading.Tasks;
using OSS.EventNode.Interfaces;

namespace OSS.EventFlow.Router
{
    public class BaseSerialRouter : BaseRouter
    {
        protected BaseSerialRouter()
        {
            RouterType = RouterType.Serial;
        }

        private readonly BaseRouter _nextRoute;

        protected BaseSerialRouter(BaseRouter nextNode)
            : this()
        {
            _nextRoute = nextNode;
        }

        internal override Task MoveNext(IExecuteData preData)
        {
            return _nextRoute?.MoveIn(preData,this);
        }
    }

}
