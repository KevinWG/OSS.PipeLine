//#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

///***************************************************************************
//*　　	文件功能描述：OSS.EventFlow -  流体追加器内部实现
//*
//*　　	创建人： Kevin
//*       创建人Email：1985088337@qq.com
//*       创建时间： 2020-11-22
//*       
//*****************************************************************************/

//#endregion

//using OSS.Pipeline.Base;
//using OSS.Pipeline.Interface;

//namespace OSS.Pipeline.Pipeline.InterImpls.Connector
//{
//    internal class InterPipelineConnector<TInContext, TOutContext> : IPipelineConnector<TInContext, TOutContext>
//    {

//        public InterPipelineConnector(BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipe)
//        {
//            Initial(this, startPipe, endPipe);
//        }
        
//        private static void Initial(IPipelineConnector<TInContext, TOutContext> pipelineAppender,
//            BaseInPipePart<TInContext> startPipe, IPipeAppender<TOutContext> endPipe)
//        {
//            pipelineAppender.StartPipe   = startPipe;
//            pipelineAppender.EndAppender = endPipe;
//        }

//        BaseInPipePart<TInContext> IPipelineConnector<TInContext, TOutContext>.StartPipe { get; set; }

//        IPipeAppender<TOutContext> IPipelineConnector<TInContext, TOutContext>.EndAppender { get; set; }
//    }
//}
