using System.Threading.Tasks;

namespace OSS.EventFlow.FlowLine
{
    public abstract class BaseFlow<TPara>
    {
        public abstract Task Entry();
        //{
        //    return Task.CompletedTask;
        //}
    }
}
