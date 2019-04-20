using System.Collections.Generic;
using OSS.EventFlow.Tasks;

namespace OSS.EventFlow.NodeWorker
{
    /// <summary>
    ///  基础工作者
    /// </summary>
    public abstract class BaseWorker<TPara>
    {
        private List<BaseTask<TPara>> _tasks = new List<BaseTask<TPara>>();

        public void AddTask()
        {

        }
    }
}
