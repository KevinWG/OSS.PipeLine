using System.Threading.Tasks;

namespace OSS.EventFlow.Router
{
    public class BaseCycleRouter : BaseRouter
    {
        protected BaseCycleRouter()
        {
            RouterType = RouterType.Cycle;
        }

        internal override Task MoveNext()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
