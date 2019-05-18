using System.Threading.Tasks;

namespace OSS.EventFlow.Router
{
    public class BaseBranchRouter : BaseRouter
    {
        protected BaseBranchRouter()
        {
            RouterType = RouterType.Branch;
        }

        internal override Task MoveNext()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
