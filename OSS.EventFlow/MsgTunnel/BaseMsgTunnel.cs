

using OSS.EventFlow.Interfaces;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    public abstract class BaseMsgTunnel:IPipe
    {
        public void InterInput(FlowContext context)
        {
            throw new System.NotImplementedException();
        }

        public void InterOutput(FlowContext context)
        {
            throw new System.NotImplementedException();
        }
    }

}
