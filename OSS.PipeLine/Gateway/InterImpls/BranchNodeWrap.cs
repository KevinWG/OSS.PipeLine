using System.Threading.Tasks;
using OSS.Pipeline.Interface;

namespace OSS.Pipeline.Gateway.InterImpls
{
    /// <summary>
    ///  分支子节点管道
    /// </summary>
    public interface IBranchWrap //: IPipe
    {
        internal Task<TrafficResult> InterPreCall(object context, string prePipeCode);

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

        Task<TrafficResult> IBranchWrap.InterPreCall(object context,string prePipeCode)
        {
            return _pipePart.InterWatchPreCall((TContext) context,prePipeCode);
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

        Task<TrafficResult> IBranchWrap.InterPreCall(object context, string prePipeCode)
        {
            return _pipePart.InterWatchPreCall(Empty.Default,prePipeCode);
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
