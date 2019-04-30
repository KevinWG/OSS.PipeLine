using System.Threading.Tasks;
using OSS.TaskFlow.Node.Mos;

namespace OSS.TaskFlow.Node
{
 
    public abstract partial class BaseNode
    {
        #region  激活模块
        
        public Task Activate(NodeContext context)
        {
            return MoveIn(context);
        }

        /// <summary>
        ///  前置进入方法
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task MoveIn(NodeContext con)
        {
            return Task.CompletedTask;
        }

        #endregion
    }

}
