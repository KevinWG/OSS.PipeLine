using System.Threading.Tasks;

namespace OSS.Pipeline.Base
{
    /// <summary>
    ///  管道基类（空参类型）
    /// </summary>
    /// <typeparam name="TOutContext"></typeparam>
    /// <typeparam name="THandlePara"></typeparam>
    public abstract class BaseEmptyParaPipe<THandlePara, TOutContext> :
       BasePipe<EmptyContext, THandlePara, TOutContext>
    {
        /// <summary>
        /// 外部Action活动基类
        /// </summary>
        protected BaseEmptyParaPipe(PipeType pipeType) : base(pipeType)
        {
        }

        #region 内部的业务处理

        internal override Task<bool> InterStart(EmptyContext context)
        {
            return InterUtil.TrueTask;
        }

        #endregion

    }
}
