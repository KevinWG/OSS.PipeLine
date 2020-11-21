using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    public interface IPipe<TContext>
    {
        PipeType PipeType { get;}
        Task Through(TContext context);

    }
}
