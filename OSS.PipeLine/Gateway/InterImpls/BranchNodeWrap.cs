using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Gateway.InterImpls
{
    /// <summary>
    ///  分支子节点管道
    /// </summary>
    public interface IBranchWrap //: IPipe
    {
        internal Task<TrafficResult> InterPreCall(object context, string prePipeCode);

        public IPipe Pipe { get;  }

        /// <summary>
        ///  内部处理流容器初始化赋值
        /// </summary>
        /// <param name="containerFlow"></param>
        internal abstract void InterInitialContainer(IPipeLine containerFlow);

        /// <summary>
        ///  内部处理流的路由信息
        /// </summary>
        /// <returns></returns>
        internal abstract PipeRoute InterToRoute(bool isFlowSelf );
    }

    internal  class BranchNodeWrap<TContext>: IBranchWrap
    {

        public IPipe Pipe
        {
            get => _pipePart;
        }


        //public PipeType PipeType { get; }
        //public string   PipeCode { get; set; }

        public BaseInPipePart<TContext> _pipePart;

        public BranchNodeWrap(BaseInPipePart<TContext> pipePart)
        {
            _pipePart = pipePart;
        }

        Task<TrafficResult> IBranchWrap.InterPreCall(object context,string prePipeCode)
        {
            return _pipePart.InterPreCall((TContext) context,prePipeCode);
        }

        void IBranchWrap.InterInitialContainer(IPipeLine containerFlow)
        {
            _pipePart.InterInitialContainer(containerFlow);
        }

        PipeRoute IBranchWrap.InterToRoute(bool isFlowSelf)
        {
            return _pipePart.InterToRoute(isFlowSelf);
        }
    }

    internal class BranchNodeWrap : IBranchWrap
    {
        public IPipe Pipe
        {
            get => _pipePart;
        }
        
        public BaseInPipePart<Empty> _pipePart;

        public BranchNodeWrap(BaseInPipePart<Empty> pipePart)
        {
            _pipePart = pipePart;
        }

        Task<TrafficResult> IBranchWrap.InterPreCall(object context, string prePipeCode)
        {
            return _pipePart.InterPreCall(Empty.Default,prePipeCode);
        }

        void IBranchWrap.InterInitialContainer(IPipeLine containerFlow)
        {
            _pipePart.InterInitialContainer(containerFlow);
        }

        PipeRoute IBranchWrap.InterToRoute(bool isFlowSelf)
        {
            return _pipePart.InterToRoute(isFlowSelf);
        }
    }
}
