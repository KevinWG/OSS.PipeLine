using OSS.EventFlow.Mos;

namespace OSS.TaskFlow.Tests.Activities.Apply
{
    public class ApplyContext:FlowContext<string>
    {
        public ApplyContext()
        {
            id = "test_flow_1";
        }
    }
}
