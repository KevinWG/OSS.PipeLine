
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interfaces
{
    internal interface IPipe
    {
        void InterInput(FlowContext context);



        void InterOutput(FlowContext context);
    }
}
