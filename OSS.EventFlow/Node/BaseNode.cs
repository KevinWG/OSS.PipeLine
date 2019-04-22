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


        //  TODO  任务时序处理（顺序，图序）
        public abstract Task Call(TPara para);
        //{
        //    // todo 执行
        //    return Task.CompletedTask;
        //}
    }
}
