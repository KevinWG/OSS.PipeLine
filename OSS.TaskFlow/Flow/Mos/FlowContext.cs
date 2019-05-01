﻿using OSS.TaskFlow.Flow.MetaMos;
using OSS.TaskFlow.Node.Mos;

namespace OSS.TaskFlow.Flow.Mos
{
    public class FlowContext:NodeContext
    {

        /// <summary>
        ///  当前流元信息
        /// </summary>
        public FlowMeta flow_meta { get; set; }
    }



}