using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.EventFlow.Tasks.Mos;

namespace OSS.EventFlow.Tasks
{
    public abstract class BaseTask
    {
        public BaseTask(TaskConfig config)
        {
        }

        /// <summary>
        /// 执行任务  
        /// </summary>
        /// <param name="doTimes"> 执行次数 </param>
        public virtual ResultMo<object> Do(int doTimes = 0)
        {
            return new ResultMo<object>(ResultTypes.UnKnowOperate,"Do ");
        }

        /// <summary>
        ///  执行失败
        ///   如果设置了重试配置，会在重试失败后调用
        /// </summary>
        public void Failed(int doTimes = 0)
        {
        }

        /// <summary>
        ///  回退操作
        /// </summary>
        public void Revert()
        {
        }
    }
}