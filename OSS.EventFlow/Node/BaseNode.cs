using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSS.EventFlow.Node
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract class BaseNode<TPara>
    {
        private List<string> _taskCodes = new List<string>();

        public abstract Task Call(TPara para);
        //{
        //    // todo 执行
        //    return Task.CompletedTask;
        //}
    }
}
