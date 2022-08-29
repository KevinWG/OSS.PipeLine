using System.Threading.Tasks;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Gateway.InterImpls
{
    /// <summary>
    ///  分支子节点管道
    /// </summary>
    internal interface IBranchWrap //: IPipe
    {
        internal Task<TrafficSignal> InterPreCall(object context);

        /// <summary>
        ///  管道基础信息
        /// </summary>
        public IPipeMeta Pipe { get;  }

        /// <summary>
        ///  内部处理流容器初始化赋值
        /// </summary>
        /// <param name="containerFlow"></param>
        internal abstract void InterInitialContainer(IPipeLine containerFlow);

        /// <summary>
        ///  内部处理流的路由信息
        /// </summary>
        /// <returns></returns>
        internal abstract void InterFormatLink(string prePipeCode, bool isSelf );
    }

    internal  class BranchNodeWrap<TContext>: IBranchWrap
    {

        public IPipeMeta Pipe
        {
            get => _pipePart;
        }
        

        public IPipeInPart<TContext> _pipePart;

        public BranchNodeWrap(IPipeInPart<TContext> pipePart)
        {
            _pipePart = pipePart;
        }

        Task<TrafficSignal> IBranchWrap.InterPreCall(object context)
        {
            return _pipePart.InterWatchPreCall((TContext) context);
        }

        void IBranchWrap.InterInitialContainer(IPipeLine containerFlow)
        {
            _pipePart.InterInitialContainer(containerFlow);
        }

        void IBranchWrap.InterFormatLink(string prePipeCode, bool isSelf )
        { 
            _pipePart.InterFormatLink(prePipeCode,isSelf);
        }
    }

    internal class BranchNodeWrap : IBranchWrap
    {
        public IPipeMeta Pipe
        {
            get => _pipePart;
        }
        
        public IPipeInPart<Empty> _pipePart;

        public BranchNodeWrap(IPipeInPart<Empty> pipePart)
        {
            _pipePart = pipePart;
        }

        Task<TrafficSignal> IBranchWrap.InterPreCall(object context)
        {
            return _pipePart.InterWatchPreCall(Empty.Default);
        }

        void IBranchWrap.InterInitialContainer(IPipeLine containerFlow)
        {
            _pipePart.InterInitialContainer(containerFlow);
        }

        void IBranchWrap.InterFormatLink(string prePipeCode, bool isSelf)
        {
            _pipePart.InterFormatLink(prePipeCode, isSelf);
        }
    }
}
