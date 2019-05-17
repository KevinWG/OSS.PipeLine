using System.Threading.Tasks;
using OSS.Common.ComModels;

namespace OSS.EventFlow
{
    public abstract partial class BaseFlow<TDomain>
    {
        public void Link()
        {
        }

        public async Task<ResultMo> Enter(string nodeId, string  data)
        {
            return new ResultMo();
        }

        public abstract Task End();
    }

}
