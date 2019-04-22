using System.Threading.Tasks;

namespace OSS.EventFlow.FlowLine
{
    public abstract class BaseFlow
    {


        public abstract Task Entry();


        public abstract Task End();
        //public abstract Task Entry();
        //{
        //    return Task.CompletedTask;
        //}
    }
}
