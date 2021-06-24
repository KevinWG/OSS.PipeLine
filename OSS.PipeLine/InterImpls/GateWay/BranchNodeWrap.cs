using System.Threading.Tasks;
using OSS.Pipeline.Base;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.InterImpls.GateWay
{
    /// <summary>
    ///  分支子节点管道
    /// </summary>
    public interface IBranchNodePipe:IPipe
    {
        internal Task<TrafficResult> InterPreCall(object context);

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

    internal  class BranchNodeWrap<TContext>: IBranchNodePipe
    {
        public PipeType                 PipeType { get; }
        public string                   PipeCode { get; set; }
        
        public BaseInPipePart<TContext> _pipePart;

        public BranchNodeWrap(BaseInPipePart<TContext> pipePart)
        {
            PipeType = pipePart.PipeType;
            PipeCode = pipePart.PipeCode;

            _pipePart = pipePart;
        }

        Task<TrafficResult> IBranchNodePipe.InterPreCall(object context)
        {
            return _pipePart.InterPreCall((TContext) context);
        }

        void IBranchNodePipe.InterInitialContainer(IPipeLine containerFlow)
        {
            _pipePart.InterInitialContainer(containerFlow);
        }

        PipeRoute IBranchNodePipe.InterToRoute(bool isFlowSelf)
        {
            return _pipePart.InterToRoute(isFlowSelf);
        }
    }

    internal class BranchNodeWrap : IBranchNodePipe
    {

        public PipeType PipeType { get; }
        public string   PipeCode { get; set; }

        public BaseInPipePart<Empty> _pipePart;

        public BranchNodeWrap(BaseInPipePart<Empty> pipePart)
        {
            PipeType = pipePart.PipeType;
            PipeCode = pipePart.PipeCode;

            _pipePart = pipePart;
        }

        Task<TrafficResult> IBranchNodePipe.InterPreCall(object context)
        {
            return _pipePart.InterPreCall(Empty.Default);
        }

        void IBranchNodePipe.InterInitialContainer(IPipeLine containerFlow)
        {
            _pipePart.InterInitialContainer(containerFlow);
        }

        PipeRoute IBranchNodePipe.InterToRoute(bool isFlowSelf)
        {
            return _pipePart.InterToRoute(isFlowSelf);
        }
    }
}
