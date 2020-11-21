using System;
using OSS.EventFlow.Interfaces;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    public class BaseActivity : IPipe
    {
        public void InterInput(FlowContext context)
        {
            throw new NotImplementedException();
        }

        public void InterOutput(FlowContext context)
        {
            throw new NotImplementedException();
        }
    }
}
